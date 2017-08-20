using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ProcessPlayer.Content
{
    public class Variables : INotifyPropertyChanged
    {
        #region private variables

        private ObservableDictionary<string, object> _var;

        #endregion

        #region internal methods

        internal void RaisePropertyChanged(string propertyName)
        {
            object val;

            if (PropertyChanged != null && !Var.TryGetValue(propertyName, out val))
                PropertyChanged(Content, new PropertyChangedEventArgs(propertyName));

            if (Content != null && Content.Children != null)
                foreach (var c in Content.Children)
                    c.Vars.RaisePropertyChanged(propertyName);
        }

        #endregion

        #region private methods

        private void raisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(Content, new PropertyChangedEventArgs(propertyName));

            if (Content != null && Content.Children != null)
                foreach (var c in Content.Children)
                    c.Vars.RaisePropertyChanged(propertyName);
        }

        #endregion

        #region properties

        public ProcessContent Content { get; set; }

        private Variables Parent { get { return Content != null && Content.Parent != null ? Content.Parent.Vars : null; } }

        private ObservableDictionary<string, object> Var
        {
            get
            {
                if (_var == null)
                {
                    _var = new ObservableDictionary<string, object>();
                    _var.CollectionChanged += OnCollectionChanged;
                }
                return _var;
            }
        }

        public object this[string name]
        {
            get
            {
                object val;
                Variables vars = this;

                do
                {
                    if (vars.Var.TryGetValue(name, out val))
                        break;
                }
                while ((vars = vars.Parent) != null);

                return val;
            }
            set
            {
                object val;

                if (!Var.TryGetValue(name, out val) || !Equals(val, value))
                {
                    Var[name] = value;

                    raisePropertyChanged(name);
                }
            }
        }

        #endregion

        #region constructors

        public Variables()
        {
        }
        public Variables(ProcessContent content)
        {
            Content = content;
        }

        #endregion

        #region events

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (KeyValuePair<string, object> v in e.NewItems)
                        raisePropertyChanged(v.Key);
                    break;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
