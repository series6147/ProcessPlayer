using Calculator.Views;
using ProcessPlayer.Data.Common;
using ProcessPlayer.Engine;
using ProcessPlayer.Windows;
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
            _contents["MainFrame"] = new MainFrame();
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
                    case "MainFrame":
                        _contents[tag] = Content = new MainFrame();
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
            navigate("MainFrame");
        }

        #endregion
    }
}
