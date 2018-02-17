using GUIContainer.Views;
using ProcessPlayer.Windows;
using ProcessPlayer.Windows.Interfaces;
using System;

namespace GUIContainer.ViewModels
{
    public class Page2ViewModel : ViewModelBase, IView
    {
        #region private variables

        private Page2 _view;
        private ViewContainer _container;

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

            Container = view.processViewContainer;
            View = view;
        }

        #endregion

        #region IView Members

        public ViewContainer Container
        {
            get { return _container; }
            set
            {
                if (_container != value)
                {
                    _container = value;

                    RaisePropertyChanged("Container");
                }
            }
        }

        #endregion
    }
}
