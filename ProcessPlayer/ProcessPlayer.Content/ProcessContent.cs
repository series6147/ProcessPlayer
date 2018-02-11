using Newtonsoft.Json;
using ProcessPlayer.Content.Common;
using ProcessPlayer.Content.Converters;
using ProcessPlayer.Data.CodeGen;
using ProcessPlayer.Data.CodeGen.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessPlayer.Content
{
    public abstract class ProcessContent : INotifyPropertyChanged
    {
        #region public constants

        public const string _Canceled = "canceled";
        public const string _CanceledAll = "canceled all";
        public const string _DataComming = "data coming";
        public const string _ExecuteFinished = "execute finished";
        public const string _ExecuteStarted = "execute started";
        public const string _Initialization = "initialization";

        #endregion

        #region private variables

        private bool _isConsole;
        private bool _isDisabled;
        private bool _isExecuted;
        private bool _isInitialized;
        private DataExchangeObject[] _outputBuffer;
        private IEnumerable<ProcessContent> _children;
        private IEnumerable<ProcessContent> _incomingLinks;
        private IEnumerable<ProcessContent> _outgoingLinks;
        private IEnumerable<string> _ignoreCalls;
        private ObservableDictionary<string, DataExchangeObject[]> _incomingDataBuffer;
        private Root _root;
        private string _method;
        private string _status;
        private Variables _vars;

        #endregion

        #region protected variables

        protected CancellationTokenSource _cancellationToken;
        protected readonly HashSet<string> _ignoreCallsHash = new HashSet<string>();

        #endregion

        #region programming methods

        public DataExchangeObject createExchangeObject()
        {
            return new DataExchangeObject();
        }

        public async void execute()
        {
            if (Root != null && (TriggerMode == TriggerMode.Any || IncomingLinks.All(r => IncomingDataBuffer.ContainsKey(r.ID) || _ignoreCallsHash.Contains(r.ID))))
                OutputBuffer = await ExecuteAsync();
        }

        public IEnumerable<ProcessContent> getAncestors()
        {
            if (Parent != null)
            {
                yield return Parent;

                foreach (var p in Parent.getAncestors())
                    yield return p;
            }
        }

        public IEnumerable<ProcessContent> getChildren()
        {
            return Children;
        }

        public IEnumerable<ProcessContent> getDescendants()
        {
            if (Children != null)
                foreach (var c in Children)
                {
                    yield return c;

                    foreach (var d in c.getDescendants())
                        yield return d;
                }
        }

        public IEnumerable<ProcessContent> getDescendantsAndSelf()
        {
            yield return this;

            if (Children != null)
                foreach (var c in Children)
                    foreach (var d in c.getDescendantsAndSelf())
                        yield return d;
        }

        public ProcessContent getChildrenByID(string id)
        {
            return Children.FirstOrDefault(d => d.ID == id);
        }

        public ProcessContent getContentByID(string id)
        {
            return Root.getDescendantsAndSelf().FirstOrDefault(d => d.ID == id);
        }

        public IEnumerable<ProcessContent> getContentsByIDs(IEnumerable<string> ids)
        {
            return
                from id in ids
                join c in Root.getDescendantsAndSelf() on id equals c.ID
                select c;
        }

        public ObservableDictionary<string, DataExchangeObject[]> getInput()
        {
            return IncomingDataBuffer;
        }

        public void log(string message)
        {
            Root.Log.Debug(string.Format("{0}:{1}", ID, message));
        }

        public void logFormat(string message, params object[] args)
        {
            Root.Log.Debug(string.Format("{0}:{1}", ID, string.Format(message, args)));
        }

        public void msg(string message)
        {
            Root.Log.Debug(message);
        }

        public void resetInput()
        {
            IncomingDataBuffer.Clear();
        }

        public void setInput(IDictionary<string, IEnumerable<object>> input)
        {
            IncomingDataBuffer.Clear();

            foreach (var kvp in input)
            {
                IncomingDataBuffer.Add(kvp.Key, kvp.Value.Select(v => new DataExchangeObject() { Data = v }).ToArray());
            }
        }

        #endregion

        #region protected methods

        protected CommonDelegate BuildDelegate(string text)
        {
            var codeGen = new JScriptGen();
            var sbOut = new StringBuilder();

            codeGen.Generate(text, sbOut, null, ' ', 1, "Perform(this, vars, globals, params)");

            return PythonScriptEngine.Translate<CommonDelegate>(sbOut.ToString(), "Perform");
        }

        protected DataExchangeObject[] GetInputAsArray()
        {
            return IncomingDataBuffer.ToArraySync().SelectMany(kvp => kvp.Value).ToArray();
        }

        protected virtual void RaiseGlobalsChanged(string variableName)
        {
            try
            {
                if (OnGlobalsChangedDlg != null)
                    OnGlobalsChangedDlg(this, Vars, Globals, new Dictionary<string, object>() { { "name", variableName }, { "value", Globals[variableName] } });

                foreach (var c in Children)
                    c.RaiseGlobalsChanged(variableName);
            }
            catch (Exception ex)
            {
                Root.Log.Error(string.Format("{0}:{1}={2} - globals changed", ID, variableName, Globals[variableName]), ex);

                Cancel();
            }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (System.Windows.Application.Current != null)
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                })).Wait();
        }

        protected virtual void RaisePropertyChangedAsync(string propertyName)
        {
            if (System.Windows.Application.Current != null)
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }));
        }

        #endregion

        #region public methods

        public virtual bool Allowed()
        {
            return true;
        }

        public virtual void Cancel()
        {
            CancellationToken.Cancel();

            Root.Log.Info(string.Format("{0} - {1}", ID, _CanceledAll));

            if (OnCancelledDlg != null)
                OnCancelledDlg(this, Vars, Globals, new Dictionary<string, object>());
        }

        public virtual async Task Diagnostics()
        {
            await Task.Run(() =>
            {
                Task.WaitAll(Children.Select(c => c.Diagnostics()).ToArray());
            });
        }

        public virtual async Task Dispose()
        {
            await Task.Run(() =>
            {
                Task.WaitAll(Children.Select(c => c.Dispose()).ToArray());
            });
        }

        public virtual async Task<DataExchangeObject[]> ExecuteAsync()
        {
            var token = CancellationToken.Token;

            if (token.IsCancellationRequested)
                return null;

            return await Task<DataExchangeObject[]>.Run(() =>
            {
                IsExecuted = true;

                DataExchangeObject[] res;

                try
                {
                    foreach (var r in OutgoingLinks)
                        r.IncomingDataBuffer.Remove(ID);

                    res = GetInputAsArray();

                    if (res != null)
                        foreach (var r in OutgoingLinks)
                            r.IncomingDataBuffer[ID] = res;
                }
                finally
                {
                    IsExecuted = false;
                }

                return res;
            }, token);
        }

        public virtual async Task Initialize()
        {
            await Task.Run(() =>
            {
                if (!_isInitialized)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(OnCancelled))
                            OnCancelledDlg += BuildDelegate(OnCancelled);

                        if (!string.IsNullOrEmpty(OnDataComming))
                            OnDataCommingDlg += BuildDelegate(OnDataComming);

                        if (!string.IsNullOrEmpty(OnExecuteFinished))
                            OnExecuteFinishedDlg += BuildDelegate(OnExecuteFinished);

                        if (!string.IsNullOrEmpty(OnExecuteStarted))
                            OnExecuteStartedDlg += BuildDelegate(OnExecuteStarted);

                        if (!string.IsNullOrEmpty(OnVariableChanged))
                            OnVariableChangedDlg += BuildDelegate(OnVariableChanged);

                        if (!string.IsNullOrEmpty(OnInitialize))
                        {
                            OnInitializeDlg += BuildDelegate(OnInitialize);
                            OnInitializeDlg(this, Vars, Globals, new Dictionary<string, object>());
                        }

                        Task.WaitAll(Children.Select(c => c.Initialize()).ToArray());
                    }
                    catch (Exception ex)
                    {
                        Root.Log.Error(string.Format("{0} - {1}\r\n{2}", ID, _Initialization, ex));

                        Cancel();
                    }

                    _isInitialized = true;
                }
            });
        }

        public virtual void RaiseDataComming()
        {
            Root.Log.Info(string.Format("{0} - {1}", ID, _DataComming));
            Root.RaiseDataComming(ID, this);

            if (OnDataCommingDlg != null)
                try
                {
                    OnDataCommingDlg(this, Vars, Globals, new Dictionary<string, object>());
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - {1}\r\n{2}", ID, _DataComming, ex));

                    Cancel();
                }
        }

        public virtual void RaiseExecuteFinished()
        {
            if (AutoResetOutputs)
                foreach (var r in OutgoingLinks)
                    r.IncomingDataBuffer.Remove(ID);

            if (OnExecuteFinishedDlg != null)
                try
                {
                    OnExecuteFinishedDlg(this, Vars, Globals, new Dictionary<string, object>());
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - {1}\r\n{2}", ID, _ExecuteFinished, ex));

                    Cancel();
                }
        }

        public virtual void RaiseExecuteStarted()
        {
            Root.Log.Info(string.Format("{0} - {1}", ID, _ExecuteStarted));
            Root.RaiseExecuteStarted(ID, this);

            if (OnExecuteStartedDlg != null)
                try
                {
                    OnExecuteStartedDlg(this, Vars, Globals, new Dictionary<string, object>());
                }
                catch (Exception ex)
                {
                    Root.Log.Error(string.Format("{0} - {1}\r\n{2}", ID, _ExecuteStarted, ex));

                    Cancel();
                }
        }

        public virtual async Task Reset()
        {
            IncomingDataBuffer.Clear();

            await Task.Run(() =>
            {
                Task.WaitAll(Children.Select(c => c.Reset()).ToArray());
            });
        }

        #endregion

        #region properties

        public bool AutoResetOutputs { get; set; }

        [JsonIgnore]
        public virtual CancellationTokenSource CancellationToken
        {
            get
            {
                if (Parent != null && Parent.CancellationToken != null)
                    return Parent.CancellationToken;

                if (_cancellationToken == null)
                    _cancellationToken = new CancellationTokenSource();
                return _cancellationToken;
            }
            set { _cancellationToken = value; }
        }

        [JsonConverter(typeof(JsonProcessContentsConverter))]
        public IEnumerable<ProcessContent> Children
        {
            get
            {
                if (_children == null)
                    _children = new ProcessContent[] { };
                return _children;
            }
            set
            {
                _children = value;

                foreach (var c in Children)
                {
                    c.Parent = this;
                    c.Root = Root;
                }
            }
        }

        public string Comment { get; set; }

        [JsonIgnore]
        public IEnumerable<ProcessContent> Descendants
        {
            get { return getDescendants(); }
        }

        [JsonIgnore]
        public virtual Variables Globals { get { return Root.Globals; } }

        public string ID { get; set; }

        public IEnumerable<string> IgnoreCalls
        {
            get { return _ignoreCalls; }
            set
            {
                _ignoreCalls = value;
                _ignoreCallsHash.Clear();

                if (_ignoreCalls != null)
                    foreach (var v in _ignoreCalls.Distinct())
                        _ignoreCallsHash.Add(v);
            }
        }

        [JsonIgnore]
        public ObservableDictionary<string, DataExchangeObject[]> IncomingDataBuffer
        {
            get
            {
                if (_incomingDataBuffer == null)
                {
                    _incomingDataBuffer = new ObservableDictionary<string, DataExchangeObject[]>();
                    _incomingDataBuffer.CollectionChanged += OnIncomingDataBuffer_CollectionChanged;
                }
                return _incomingDataBuffer;
            }
            set
            {
                if (_incomingDataBuffer != null)
                    _incomingDataBuffer.CollectionChanged -= OnIncomingDataBuffer_CollectionChanged;

                _incomingDataBuffer = value ?? new ObservableDictionary<string, DataExchangeObject[]>();

                if (_incomingDataBuffer != null)
                    _incomingDataBuffer.CollectionChanged += OnIncomingDataBuffer_CollectionChanged;
            }
        }

        [JsonIgnore]
        public IEnumerable<ProcessContent> IncomingLinks
        {
            get
            {
                if (_incomingLinks == null)
                    _incomingLinks = new ProcessContent[] { };
                return _incomingLinks;
            }
            set { _incomingLinks = value; }
        }

        [JsonIgnore]
        public bool IsConsole
        {
            get { return _isConsole; }
            set
            {
                if (_isConsole != value)
                {
                    _isConsole = value;

                    RaisePropertyChangedAsync("IsConsole");

                    foreach (var c in Children)
                        c.IsConsole = value;
                }
            }
        }

        [JsonIgnore]
        public bool IsDisabled
        {
            get { return _isDisabled; }
            set
            {
                if (_isDisabled != value)
                {
                    _isDisabled = value;

                    RaisePropertyChangedAsync("IsDisabled");
                }
            }
        }

        [JsonIgnore]
        public bool IsExecuted
        {
            get { return _isExecuted; }
            protected set
            {
                if (_isExecuted != value)
                {
                    if (_isExecuted = value)
                        RaiseExecuteStarted();
                    else
                        RaiseExecuteFinished();

                    RaisePropertyChangedAsync("IsExecuted");
                }
            }
        }

        public string Method
        {
            get { return _method; }
            set
            {
                if (_method != value)
                {
                    _method = value;

                    RaisePropertyChangedAsync("Method");
                }
            }
        }

        public string OnCancelled { get; set; }

        public string OnDataComming { get; set; }

        public string OnExecuteFinished { get; set; }

        public string OnExecuteStarted { get; set; }

        public string OnInitialize { get; set; }

        public string OnVariableChanged { get; set; }

        public IEnumerable<string> OutgoingIDs { get; set; }

        [JsonIgnore]
        public DataExchangeObject[] OutputBuffer
        {
            get { return _outputBuffer; }
            set
            {
                if (_outputBuffer != value)
                {
                    _outputBuffer = value;

                    RaisePropertyChangedAsync("OutputBuffer");
                }
            }
        }

        [JsonIgnore]
        public IEnumerable<ProcessContent> OutgoingLinks
        {
            get
            {
                if (_outgoingLinks == null)
                    _outgoingLinks = new ProcessContent[] { };
                return _outgoingLinks;
            }
            set { _outgoingLinks = value; }
        }

        [JsonIgnore]
        public ProcessContent Parent { get; set; }

        [JsonIgnore]
        public Root Root
        {
            get { return _root; }
            set
            {
                if (_root != value)
                {
                    _root = value;

                    foreach (var c in Children)
                        c.Root = Root;
                }
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;

                    RaisePropertyChangedAsync("Status");
                }
            }
        }

        public ProcessContent this[string value]
        {
            get { return getContentByID(value); }
        }

        public TriggerMode TriggerMode { get; set; }

        [JsonIgnore]
        public Variables Vars
        {
            get
            {
                if (_vars == null)
                {
                    _vars = new Variables(this);
                    _vars.PropertyChanged += OnVariable_PropertyChanged;
                }
                return _vars;
            }
        }

        public string[] Views { get; set; }

        #endregion

        #region events

        public event CommonDelegate OnCancelledDlg;
        public event CommonDelegate OnDataCommingDlg;
        public event CommonDelegate OnExecuteFinishedDlg;
        public event CommonDelegate OnExecuteStartedDlg;
        public event CommonDelegate OnGlobalsChangedDlg;
        public event CommonDelegate OnInitializeDlg;
        public event CommonDelegate OnVariableChangedDlg;

        protected virtual void OnIncomingDataBuffer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    RaiseDataComming();

                    if (e.NewItems.Cast<KeyValuePair<string, DataExchangeObject[]>>().All(kvp => _ignoreCallsHash.Contains(kvp.Key)))
                        return;

                    execute();
                    break;
            }
        }

        private void OnVariable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Equals(sender, this))
                Root.Log.Info(string.Format("{0}:{1}={2} - variable changed", ID, e.PropertyName, Vars[e.PropertyName]));

            if (OnVariableChangedDlg != null)
                OnVariableChangedDlg(this, Vars, Globals, new Dictionary<string, object>() { { "name", e.PropertyName }, { "value", Vars[e.PropertyName] } });
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public delegate object CommonDelegate(ProcessContent content, Variables wars, Variables globals, Dictionary<string, object> dict);

    public enum TriggerMode
    {
        All = 0,
        Any = 1
    }
}
