using ID_Mark120.Views;
using ProcessPlayer.Windows;
using ProcessPlayer.Windows.Interfaces;
using System;
using Zeiss.Controls;

namespace ID_Mark120.ViewModels
{
    public class ProcessEngrDisplayViewModel : ViewModelBase, IView
    {
        #region private variables

        private ProcessEngrDisplay _view;
        private ViewContainer _container;

        #endregion

        #region private methods

        private void bindLogger()
        {
            if (Container == null)
                Container.Appending -= OnContainer_Appending;
            else
                Container.Appending += OnContainer_Appending;
        }

        #endregion

        #region properties

        public ProcessEngrDisplay View
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

        public ProcessEngrDisplayViewModel(ProcessEngrDisplay view)
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
                    _container = value;

                    RaisePropertyChanged("Container");

                    bindLogger();
                }
            }
        }

        #endregion
    }
}
