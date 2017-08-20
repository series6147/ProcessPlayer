using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using System;
using System.Collections;
using System.Data;

namespace ProcessPlayer.Data.CodeGen
{
    public static class PythonScriptEngine
    {
        #region private variables

        private static readonly ScriptEngine _engine;

        #endregion

        #region public static methods

        public static T Translate<T>(string expression, string method)
        {
            ScriptScope scope = _engine.CreateScope();
            ScriptSource source = _engine.CreateScriptSourceFromString(expression, SourceCodeKind.Statements);
            var module = source.Compile();

            module.Execute(scope);

            return scope.GetVariable<T>(method);
        }

        #endregion

        #region properties

        public static ScriptRuntime Runtime { get; private set; }

        #endregion

        #region contructors

        static PythonScriptEngine()
        {
            var setup = IronPython.Hosting.Python.CreateRuntimeSetup(null);

            Runtime = new ScriptRuntime(setup);

            _engine = IronPython.Hosting.Python.GetEngine(Runtime);

            var context = HostingHelpers.GetLanguageContext(_engine) as PythonContext;
            var hooks = context.SystemState.Get__dict__()["path_hooks"] as IList;

            hooks.Clear();

            Runtime.LoadAssembly(typeof(DBNull).Assembly);
            Runtime.LoadAssembly(typeof(IDataReader).Assembly);
            Runtime.LoadAssembly(typeof(IDictionary).Assembly);
            Runtime.LoadAssembly(typeof(Functions.AggregateExtensions).Assembly);
        }

        #endregion
    }
}
