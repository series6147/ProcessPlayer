using log4net.Core;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ProcessPlayer.Windows.Controls
{
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    public class LogListView : Control
    {
        #region private variables

        private ListBox _listBox;

        #endregion

        #region private methods

        private static void onItemsSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trg = (LogListView)d;

            if (e.OldValue is ObservableCollection<LoggingEvent>)
                ((ObservableCollection<LoggingEvent>)e.OldValue).CollectionChanged -= trg.OnLogListView_CollectionChanged;

            if (e.NewValue is ObservableCollection<LoggingEvent>)
                ((ObservableCollection<LoggingEvent>)e.NewValue).CollectionChanged += trg.OnLogListView_CollectionChanged;
        }

        #endregion

        #region public methods

        public void ScrollToEnd()
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (_listBox != null)
                        _listBox.ScrollIntoView(_listBox.Items[_listBox.Items.Count - 1]);
                }));
        }

        #endregion

        #region dependency properties

        public static readonly DependencyProperty ItemsLimitProperty = DependencyProperty.Register("ItemsLimit", typeof(int), typeof(LogListView)
            , new PropertyMetadata(1000));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<LoggingEvent>), typeof(LogListView)
            , new PropertyMetadata(null, onItemsSourceChangedCallback));

        #endregion

        #region properties

        public int ItemsLimit
        {
            get { return (int)GetValue(ItemsLimitProperty); }
            set { SetValue(ItemsLimitProperty, value); }
        }

        public ObservableCollection<LoggingEvent> ItemsSource
        {
            get { return (ObservableCollection<LoggingEvent>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value ?? new ObservableCollection<LoggingEvent>()); }
        }

        #endregion

        #region constructors

        public LogListView()
        {
            DefaultStyleKey = typeof(LogListView);
            ItemsSource = new ObservableCollection<LoggingEvent>();

            Loaded += OnLoaded;
        }

        #endregion

        #region events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            _listBox = GetTemplateChild("PART_ListBox") as ListBox;
        }

        private void OnLogListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            while (ItemsSource.Count > ItemsLimit)
                                ItemsSource.RemoveAt(0);

                            if (_listBox != null)
                                _listBox.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
                        }));
                    break;
            }
        }

        #endregion
    }
}
