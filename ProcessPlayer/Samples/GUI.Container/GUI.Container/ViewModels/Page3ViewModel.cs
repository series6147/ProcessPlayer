using GUIContainer.Views;
using ProcessPlayer.Windows;
using System;
using System.Windows;

namespace GUIContainer.ViewModels
{
    public class Page3ViewModel : ViewModelBase
    {
        #region private variables

        private Page3 _view;
        private ViewContainer _container;

        #endregion

        #region properties

        public Page3 View
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

        public Page3ViewModel(Page3 view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            View = view;

            ProcessMemoryAppender.Current.Appending += OnLogEvent_Appending;
        }

        #endregion

        #region events

        private void OnLogEvent_Appending(object sender, LoggingEventArgs e)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    View.logListView.ItemsSource.Add(e.Event);
                }));
        }

        #endregion
    }
}
