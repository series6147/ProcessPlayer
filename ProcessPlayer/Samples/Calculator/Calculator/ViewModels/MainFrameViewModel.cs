using Calculator.Views;
using ProcessPlayer.Data.Common;
using ProcessPlayer.Engine;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Calculator.ViewModels
{
    public class MainFrameViewModel : ViewModelBase
    {
        #region private variables

        private Control _content;
        private readonly Dictionary<string, Control> _contents = new Dictionary<string, Control>();
        private RelayCommand _navigationCommand;

        #endregion

        #region private methods

        private void initialize()
        {
            _contents["Diagnostics"] = new Diagnostics();
            _contents["Setup and Calibration"] = new SetupAndCalibration();
        }

        private void navigate(object parameter)
        {
            Control content = null;
            var tag = parameter as string;

            if (_contents.TryGetValue(tag, out content))
                Content = content;
            else
                switch (tag)
                {
                    case "Diagnostics":
                        _contents[tag] = Content = new Diagnostics();
                        break;
                    case "Help":
                        _contents[tag] = Content = new Help();
                        break;
                    case "Process":
                        _contents[tag] = Content = new Process();
                        break;
                    case "Setup and Calibration":
                        _contents[tag] = Content = new SetupAndCalibration();
                        break;
                    case "Task Management":
                        _contents[tag] = Content = new TaskManagement();
                        break;
                }
        }

        #endregion

        #region commands

        public RelayCommand NavigationCommand
        {
            get
            {
                if (_navigationCommand == null)
                    _navigationCommand = new RelayCommand(navigate);
                return _navigationCommand;
            }
        }

        #endregion

        #region properties

        public Control Content
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

        public ScriptPlayer ScriptPlayer { get { return MainWindowViewModel.Current.ScriptPlayer; } }

        #endregion

        #region constructors

        public MainFrameViewModel()
        {
            initialize();
            navigate("Process");
        }

        #endregion
    }
}
