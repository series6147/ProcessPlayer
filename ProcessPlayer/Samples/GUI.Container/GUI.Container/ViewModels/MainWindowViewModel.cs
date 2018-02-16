using ProcessPlayer.Engine;
using ProcessPlayer.Windows;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ViewContainer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region private variables

        private MainWindow _view;
        private object _content;
        private static MainWindowViewModel _current;

        #endregion

        #region private methods

        private async Task executeProcess()
        {
            var args = Environment.GetCommandLineArgs();
            var json = string.Empty;

            ScriptPlayer = ScriptPlayer.Default;
            ScriptPlayer.Log.Info("Process started.");

            foreach (var arg in args.Skip(1))
            {
                using (var reader = File.OpenText(arg))
                {
                    json = reader.ReadToEnd();
                }

                await ScriptPlayer.PrepareAndDiagnostics(json, arg, 120000);

                if (ScriptPlayer.IsPrepared)
                    await ScriptPlayer.Play();
            }
        }

        #endregion

        #region properties

        public object Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;

                    RaisePropertyChanged("Content");
                }
            }
        }

        public static MainWindowViewModel Current
        {
            get { return _current; }
        }

        public ScriptPlayer ScriptPlayer { get; private set; }

        #endregion

        #region constructors

        public MainWindowViewModel(MainWindow view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            _current = this;
            _view = view;

            executeProcess();
        }

        #endregion
    }
}
