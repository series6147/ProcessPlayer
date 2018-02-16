using ID_Mark120.Views;
using log4net.Core;
using ProcessPlayer.Windows;
using ProcessPlayer.Windows.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Zeiss.Controls;

namespace ID_Mark120.ViewModels
{
    public class DiagnosticsLogViewModel : ViewModelBase, IView
    {
        #region private variables

        private DiagnosticsLog _view;
        private int _itemsLimit;
        private ObservableCollection<LoggingEvent> _itemsSource;
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

        public int ItemsLimit
        {
            get { return _itemsLimit; }
            set
            {
                if (_itemsLimit != value)
                {
                    _itemsLimit = value;

                    RaisePropertyChanged("ItemsLimit");
                }
            }
        }

        public ObservableCollection<LoggingEvent> ItemsSource
        {
            get
            {
                if (_itemsSource == null)
                {
                    _itemsSource = new ObservableCollection<LoggingEvent>();
                    _itemsSource.CollectionChanged += OnItemsSource_CollectionChanged;
                }
                return _itemsSource;
            }
        }

        public DiagnosticsLog View
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

        public DiagnosticsLogViewModel(DiagnosticsLog view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            ItemsLimit = 10000;
            View = view;
        }

        #endregion

        #region events

        private void OnContainer_Appending(object sender, LoggingEventArgs e)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    ItemsSource.Add(e.Event);
                }));
        }

        private void OnItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            while (ItemsSource.Count > ItemsLimit)
                                ItemsSource.RemoveAt(0);
                        }));
                    break;
            }
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
