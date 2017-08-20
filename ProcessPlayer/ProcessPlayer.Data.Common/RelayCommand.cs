using System;
using System.Windows.Input;

namespace ProcessPlayer.Data.Common
{
    public class RelayCommand : ICommand
    {
        #region private variables

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion

        #region constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _canExecute = canExecute;
            _execute = execute;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}
