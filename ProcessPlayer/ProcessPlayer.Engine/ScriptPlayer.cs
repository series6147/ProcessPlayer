using log4net;
using Newtonsoft.Json;
using ProcessPlayer.Content;
using ProcessPlayer.Content.Common;
using ProcessPlayer.Content.Converters;
using ProcessPlayer.Data.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPlayer.Engine
{
    public class ScriptPlayer : INotifyPropertyChanged
    {
        #region private variables

        private ILog _log;
        private Root _root;
        private static ScriptPlayer _default;
        private Variables _globals;

        #endregion

        #region private methods

        private void prepareDeserialization(ref string script, string scriptPath)
        {
            var asms = new List<Assembly>() { Assembly.GetAssembly(typeof(ProcessContent)) };
            var sbErr = new StringBuilder();

            using (var errOut = new StringWriter(sbErr))
            {
                PegNode root;
                var parser = new JsonParser();

                while (true)
                {
                    sbErr.Clear();

                    parser.Construct(script, errOut);
                    parser.Preprocess();

                    if ((root = parser.GetRoot()) != null && root.child != null)
                    {
                        var currentDirectory = Directory.GetCurrentDirectory();
                        var fileInfo = new FileInfo(scriptPath);
                        var idsMapping = new Dictionary<string, string>();

                        Directory.SetCurrentDirectory(fileInfo.DirectoryName);

                        foreach (var n in PegCharParser.GetDescendants(root)
                            .Where(n => n.id == (int)EJsonParser.include || n.id == (int)EJsonParser.includeRelative)
                            .OrderByDescending(n => n.match.posEnd)
                            .ToArray())
                        {
                            var path = n.child.GetAsString(script).Trim('\"');

                            if (File.Exists(path))
                            {
                                if (n.id == (int)EJsonParser.include)
                                    script = string.Concat(script.Substring(0, n.match.posBeg)
                                        , File.OpenText(path).ReadToEnd()
                                        , script.Substring(n.match.posEnd));
                                else
                                {
                                    var include = File.OpenText(path).ReadToEnd();
                                    var prefix = n.child.next.GetAsString(script).Trim('\"');

                                    idsMapping.Clear();
                                    sbErr.Clear();

                                    parser.Construct(include, errOut);
                                    parser.TopElement();

                                    if ((root = parser.GetRoot()) != null && root.child != null)
                                        foreach (var o in PegCharParser.GetDescendants(root)
                                            .Where(o => o.id == (int)EJsonParser.pair && (string.Equals(o.child.GetAsString(include).Trim('\"'), "ID", StringComparison.InvariantCultureIgnoreCase) || string.Equals(o.child.GetAsString(include).Trim('\"'), "OutgoingIDs", StringComparison.InvariantCultureIgnoreCase)))
                                            .OrderByDescending(o => o.match.posEnd))
                                        {
                                            if (string.Equals(o.child.GetAsString(include).Trim('\"'), "ID", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                idsMapping[string.Format("\'{0}\'", o.child.next.GetAsString(include).Trim('\"'))] = string.Format("\'{0}.{1}\'", prefix, o.child.next.GetAsString(include).Trim('\"'));

                                                include = string.Concat(include.Substring(0, o.child.next.match.posBeg)
                                                    , string.Format("\"{0}.{1}\"", prefix, o.child.next.GetAsString(include).Trim('\"'))
                                                    , include.Substring(o.child.next.match.posEnd));
                                            }
                                            else if (o.child.next.id == (int)EJsonParser.array)
                                                foreach (var e in parser.GetNodeChildren(o.child.next).OrderByDescending(e => e.match.posEnd))
                                                    include = string.Concat(include.Substring(0, e.match.posBeg)
                                                        , string.Format("\"{0}.{1}\"", prefix, e.GetAsString(include).Trim('\"'))
                                                        , include.Substring(e.match.posEnd));
                                        }

                                    foreach (var kvp in idsMapping)
                                        include = include.Replace(kvp.Key, kvp.Value);

                                    script = string.Concat(script.Substring(0, n.match.posBeg), include, script.Substring(n.match.posEnd));
                                }
                            }
                        }

                        Directory.SetCurrentDirectory(currentDirectory);
                    }
                    else
                        break;
                }

                sbErr.Clear();

                parser.Construct(script, errOut);
                parser.TopElement();

                if ((root = parser.GetRoot()) != null)
                {
                    if (root.child.id != (int)EJsonParser.Object)
                        throw new Exception("Parsing failed. Root element should be object");

                    var scr = script;
                    var assemblyPair = root.child == null ? null : parser.GetNodeChildren(root.child.child)
                        .Where(n => n.id == (int)EJsonParser.pair
                            && n.child.next.id == (int)EJsonParser.array
                            && (string.Compare(n.child.GetAsString(scr), "Assemblies", true) == 0 || string.Compare(n.child.GetAsString(scr), "\"Assemblies\"", true) == 0))
                        .FirstOrDefault();

                    if (assemblyPair != null)
                    {
                        Assembly asm;
                        var dict = AppDomain.CurrentDomain.GetAssemblies()
                            .ToDictionary(a => a.GetName().Name, a => a);
                        var items = parser.GetNodeChildren(assemblyPair.child.next)
                            .Where(n => n.id == (int)EJsonParser.String)
                            .Select(n => n.GetAsString(scr).Trim('\"'))
                            .Distinct();

                        foreach (var item in items)
                        {
                            if (!dict.TryGetValue(item, out asm))
                                asm = Assembly.Load(item);

                            asms.Add(asm);
                        }
                    }
                }
                else
                    throw new Exception(string.Format("Parsing failed. {0}", sbErr.ToString()));
            }

            IEnumerable<Type> types = new Type[] { };
            var basicType = typeof(ProcessContent);

            foreach (var asm in asms)
                try
                {
                    types = types.Union(asm.GetTypes().Where(t => t.IsClass && t.IsPublic && t.IsSubclassOf(basicType)));
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = types.Union(ex.Types.Where(t => t != null && t.IsClass && t.IsPublic && t.IsSubclassOf(basicType)));
                }

            JsonProcessContentsConverter.SetMappingTypes(types);
        }

        private void raisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region public methods

        public async Task<IEnumerable<DataExchangeObject>> Play()
        {
            if (IsPrepared && Root != null)
            {
                await Root.Reset();

                var lastContents = Root.Children.Where(r => r.OutgoingLinks == null || !r.OutgoingLinks.Any()).ToArray();
                var collector = new Collector() { IncomingLinks = lastContents, Root = Root };

                foreach (var r in lastContents)
                    r.OutgoingLinks = new ProcessContent[] { collector };

                foreach (var r in Root.Children.Where(r => r.IncomingLinks == null || !r.IncomingLinks.Any()).ToArray())
                    r.ExecuteAsync();

                var res = await collector.ExecuteAsync();

                Root.Dispose();

                Log.Debug("Execution completed successfully.");

                return res;
            }

            return null;
        }

        public async Task PrepareAndDiagnostics(string script, string scriptPath, int millisecondsTimeout)
        {
            IsPrepared = false;

            try
            {
                prepareDeserialization(ref script, scriptPath);

                Root = JsonConvert.DeserializeObject<Root>(script);
                Root.IsConsole = IsConsole;
                Root.Log = Log;
                Root.ScriptPath = scriptPath;

                if (_globals == null)
                    _globals = new Variables();

                Root.SetGlobals(_globals);

                var lastContents = Root.Children.Where(r => r.OutgoingLinks == null || !r.OutgoingLinks.Any());

                if (lastContents.Count() == 0)
                    throw new Exception("No the ending content.");

                await Root.Initialize();
                await Root.Diagnostics();

                IsPrepared = true;

                Log.Debug("Preparation and diagnostics completed successfully.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }
        }

        #endregion

        #region properties

        public static ScriptPlayer Default
        {
            get
            {
                if (_default == null)
                    _default = new ScriptPlayer();
                return _default;
            }
        }

        public bool IsConsole { get; set; }

        public bool IsPrepared { get; private set; }

        public ILog Log
        {
            get
            {
                if (_log == null)
                    _log = LogManager.GetLogger(string.Format("{0}", (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds));
                return _log;
            }
            set { _log = value; }
        }

        public Root Root
        {
            get { return _root; }
            set
            {
                if (_root != value)
                {
                    _root = value;

                    raisePropertyChanged("Root");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
