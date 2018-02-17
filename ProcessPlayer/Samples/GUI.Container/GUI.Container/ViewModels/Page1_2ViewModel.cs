using GUIContainer.Views;
using ProcessPlayer.Windows;
using ProcessPlayer.Windows.Interfaces;
using System;

namespace GUIContainer.ViewModels
{
    public class Page1_2ViewModel : ViewModelBase, IView
    {
        #region private variables

        private Page1_2 _view;
        private ViewContainer _container;

        #endregion

        #region properties

        public Page1_2 View
        {
            get { return _view; }
            set
            {
                if (_view != value)
                {
                    _view = value;

                    RaisePropertyChanged("View");
                }
            }
        }

        #endregion

        #region constructors

        public Page1_2ViewModel(Page1_2 view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            View = view;
        }

        #endregion

        #region events

        private void OnContainer_Appending(object sender, LoggingEventArgs e)
        {
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
                    if (_container != null)
                        _container.Appending -= OnContainer_Appending;

                    _container = value;

                    if (_container != null)
                        _container.Appending += OnContainer_Appending;

                    RaisePropertyChanged("Container");
                }
            }
        }

        #endregion
    }
}
