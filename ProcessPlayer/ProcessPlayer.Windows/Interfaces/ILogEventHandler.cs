using System;

namespace ProcessPlayer.Windows.Interfaces
{
    public interface ILogEventHandler
    {
        #region events

        event EventHandler<LoggingEventArgs> Appending;

        #endregion
    }
}
