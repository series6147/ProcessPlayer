using ProcessPlayer.Content;
using ProcessPlayer.Content.Common;
using ProcessPlayer.Engine;
using ProcessPlayer.Windows.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProcessPlayer.Windows
{
    public class ViewContainer : ContentControl
    {
        #region private variables

        private Dictionary<FrameworkElement, object> _viewContext;
        private ILogEventHandler _logEventHandler;
        private ScriptPlayer _scriptPlayer;

        #endregion

        #region private methods

        private void initialize()
        {
            if (_scriptPlayer != null && _scriptPlayer.Root != null)
            {
                _scriptPlayer.Root.DataComming += OnScriptPlayer_DataComming;
                _scriptPlayer.Root.ExecuteStarted += OnScriptPlayer_DataComming;

                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        Contents = _scriptPlayer.Root.getDescendants().ToDictionary(c => c.ID, c => c);
                    }));
            }
        }

        private static void onViewsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IView context;
            var trg = (ViewContainer)d;

            if (e.OldValue is ObservableCollection<FrameworkElement>)
            {
                foreach (FrameworkElement fe in ((ObservableCollection<FrameworkElement>)e.OldValue))
                {
                    context = fe.DataContext as IView;

                    if (context != null)
                        context.Container = null;

                    if (trg.ViewContext.ContainsKey(fe))
                        trg.ViewContext.Remove(fe);
                }

                ((ObservableCollection<FrameworkElement>)e.OldValue).CollectionChanged -= trg.OnViewContainer_CollectionChanged;
            }

            if (e.NewValue is ObservableCollection<FrameworkElement>)
            {
                foreach (FrameworkElement fe in ((ObservableCollection<FrameworkElement>)e.NewValue))
                {
                    context = fe.DataContext as IView;

                    if (context != null)
                        context.Container = trg;

                    trg.ViewContext[fe] = fe.DataContext;

                    fe.DataContext = null;
                }

                ((ObservableCollection<FrameworkElement>)e.NewValue).CollectionChanged += trg.OnViewContainer_CollectionChanged;
            }
        }

        private void selectView(string id, ProcessContent content)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (Contents.TryGetValue(id, out content) && content.Views != null)
                    {
                        var view =
                            (from v in Views
                             join n in content.Views on v.GetType().Name equals n
                             select v).FirstOrDefault();

                        if (view != null)
                        {
                            FrameworkElement fe;

                            if (Content is FrameworkElement)
                            {
                                fe = (FrameworkElement)Content;

                                ViewContext[fe] = fe.DataContext;

                                fe.DataContext = null;
                                fe.Visibility = Visibility.Collapsed;
                            }

                            Content = view;

                            if (Content is FrameworkElement)
                            {
                                object context;

                                fe = (FrameworkElement)Content;

                                if (ViewContext.TryGetValue(fe, out context))
                                {
                                    fe.DataContext = context;

                                    ViewContext.Remove(fe);
                                }

                                fe.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }));
        }

        #endregion

        #region public methods

        public void SelectView(string name)
        {
            if (Application.Current != null && !string.IsNullOrEmpty(name))
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    var view = Views.FirstOrDefault(v => v.GetType().Name == name);

                    if (view != null)
                    {
                        FrameworkElement fe;

                        if (Content is FrameworkElement)
                        {
                            fe = (FrameworkElement)Content;

                            ViewContext[fe] = fe.DataContext;

                            fe.DataContext = null;
                            fe.Visibility = Visibility.Collapsed;
                        }

                        Content = view;

                        if (Content is FrameworkElement)
                        {
                            object context;

                            fe = (FrameworkElement)Content;

                            if (ViewContext.TryGetValue(fe, out context))
                            {
                                fe.DataContext = context;

                                ViewContext.Remove(fe);
                            }

                            fe.Visibility = Visibility.Visible;
                        }
                    }
                }));
        }

        #endregion

        #region dependency properties

        public static readonly DependencyProperty ContentsProperty = DependencyProperty.Register("Contents", typeof(Dictionary<string, ProcessContent>), typeof(ViewContainer)
            , new PropertyMetadata(null));

        public static readonly DependencyProperty ViewsProperty = DependencyProperty.Register("Views", typeof(ObservableCollection<FrameworkElement>), typeof(ViewContainer)
            , new PropertyMetadata(null, onViewsChangedCallback));

        #endregion

        #region properties

        public Dictionary<string, ProcessContent> Contents
        {
            get { return (Dictionary<string, ProcessContent>)GetValue(ContentsProperty); }
            set { SetValue(ContentsProperty, value ?? new Dictionary<string, ProcessContent>()); }
        }

        public ILogEventHandler LogEventHandler
        {
            get { return _logEventHandler; }
            set
            {
                if (_logEventHandler != value)
                {
                    if (_logEventHandler != null)
                        _logEventHandler.Appending -= OnLogEventHandler_Appending;

                    _logEventHandler = value;

                    if (_logEventHandler != null)
                        _logEventHandler.Appending += OnLogEventHandler_Appending;
                }
            }
        }

        public ScriptPlayer ScriptPlayer
        {
            get { return _scriptPlayer; }
            set
            {
                if (_scriptPlayer != value)
                {
                    if (_scriptPlayer != null)
                    {
                        _scriptPlayer.PropertyChanged -= OnScriptPlayer_PropertyChanged;
                        _scriptPlayer.Root.DataComming -= OnScriptPlayer_DataComming;
                        _scriptPlayer.Root.ExecuteStarted -= OnScriptPlayer_DataComming;
                    }

                    _scriptPlayer = value;

                    if (_scriptPlayer != null)
                        _scriptPlayer.PropertyChanged += OnScriptPlayer_PropertyChanged;

                    initialize();
                }
            }
        }

        private Dictionary<FrameworkElement, object> ViewContext
        {
            get
            {
                if (_viewContext == null)
                    _viewContext = new Dictionary<FrameworkElement, object>();
                return _viewContext;
            }
        }

        public ObservableCollection<FrameworkElement> Views
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(ViewsProperty); }
            set { SetValue(ViewsProperty, value ?? new ObservableCollection<FrameworkElement>()); }
        }

        #endregion

        #region constructors

        public ViewContainer()
        {
            Contents = new Dictionary<string, ProcessContent>();
            Views = new ObservableCollection<FrameworkElement>();
        }

        public ViewContainer(ScriptPlayer scriptPlayer, ILogEventHandler logEventHandler)
        {
            _logEventHandler = logEventHandler;
            _logEventHandler.Appending += OnLogEventHandler_Appending;

            _scriptPlayer = scriptPlayer;
            _scriptPlayer.PropertyChanged += OnScriptPlayer_PropertyChanged;

            Contents = new Dictionary<string, ProcessContent>();
            Views = new ObservableCollection<FrameworkElement>();
        }

        #endregion

        #region events

        public event EventHandler<LoggingEventArgs> Appending;

        private void OnLogEventHandler_Appending(object sender, LoggingEventArgs e)
        {
            if (Appending != null)
                Appending(this, e);
        }

        private void OnScriptPlayer_DataComming(object sender, ProcessContentNotifyEventArgs e)
        {
            selectView(e.ID, e.Source);
        }

        private void OnScriptPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Root")
                initialize();
        }

        private void OnViewContainer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IView context;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (FrameworkElement fe in e.NewItems.OfType<FrameworkElement>())
                    {
                        context = fe.DataContext as IView;

                        if (context != null)
                            context.Container = this;

                        ViewContext[fe] = fe.DataContext;

                        fe.DataContext = null;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (FrameworkElement fe in e.OldItems.OfType<FrameworkElement>())
                    {
                        context = fe.DataContext as IView;

                        if (context != null)
                            context.Container = null;

                        if (ViewContext.ContainsKey(fe))
                            ViewContext.Remove(fe);
                    }
                    break;
            }
        }

        #endregion
    }
}
