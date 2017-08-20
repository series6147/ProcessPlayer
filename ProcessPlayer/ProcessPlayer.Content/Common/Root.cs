using log4net;
using System;
using System.ComponentModel;

namespace ProcessPlayer.Content.Common
{
    public class Root : ProcessContent
    {
        #region private variables

        private ILog _log;
        private Variables _globals;

        #endregion

        #region public methods

        public void RaiseDataComming(string id, ProcessContent source)
        {
            if (DataComming != null)
                DataComming(this, new ProcessContentNotifyEventArgs(id, source));
        }

        public void RaiseExecuteStarted(string id, ProcessContent source)
        {
            if (ExecuteStarted != null)
                ExecuteStarted(this, new ProcessContentNotifyEventArgs(id, source));
        }

        public void SetGlobals(Variables vars)
        {
            if (_globals != vars)
            {
                if (_globals != null)
                    _globals.PropertyChanged -= OnVariable_PropertyChanged;

                _globals = vars;

                if (_globals != null)
                    _globals.PropertyChanged += OnVariable_PropertyChanged;
            }
        }

        #endregion

        #region properties

        public string[] Assemblies { get; set; }

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

        public override Variables Globals
        {
            get
            {
                if (_globals == null)
                {
                    _globals = new Variables(this);
                    _globals.PropertyChanged += OnVariable_PropertyChanged;
                }
                return _globals;
            }
        }

        public string ScriptPath { get; set; }

        #endregion

        #region constructors

        public Root()
            : base()
        {
            Root = this;
        }

        #endregion

        #region events

        public event EventHandler<ProcessContentNotifyEventArgs> DataComming;
        public event EventHandler<ProcessContentNotifyEventArgs> ExecuteStarted;

        private void OnVariable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Equals(sender, this))
                Log.Info(string.Format("{0}={1} - globals changed", e.PropertyName, Globals[e.PropertyName]));

            RaiseGlobalsChanged(e.PropertyName);
        }

        #endregion
    }

    public class ProcessContentNotifyEventArgs : EventArgs
    {
        #region properties

        public string ID { get; set; }

        public ProcessContent Source { get; set; }

        #endregion

        #region constructors

        public ProcessContentNotifyEventArgs()
        {
        }

        public ProcessContentNotifyEventArgs(string id, ProcessContent source)
        {
            ID = id;
            Source = source;
        }

        #endregion
    }
}
