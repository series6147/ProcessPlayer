using GUIContainer.Views;
using ProcessPlayer.Windows;
using System;

namespace GUIContainer.ViewModels
{
    public class Page2ViewModel : ViewModelBase
    {
        #region private variables

        private Page2 _view;

        #endregion

        #region properties

        public Page2 View
        {
            get { return _view; }
            set
            {
                if (_view != value)
                {
                    _view = value;
                    _view.processViewContainer.LogEventHandler = ProcessMemoryAppender.Current;
                    _view.processViewContainer.ScriptPlayer = MainWindowViewModel.Current == null ? null : MainWindowViewModel.Current.ScriptPlayer;

                    RaisePropertyChanged("View");
                }
            }
        }

        #endregion

        #region constructors

        public Page2ViewModel(Page2 view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            View = view;
        }

        #endregion
    }
}
