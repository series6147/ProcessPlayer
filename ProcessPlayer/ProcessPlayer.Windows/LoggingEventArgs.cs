using log4net.Core;
using System;

namespace ProcessPlayer.Windows
{
    public class LoggingEventArgs : EventArgs
    {
        #region properties

        public LoggingEvent Event { get; set; }

        #endregion

        #region constructors

        public LoggingEventArgs()
        {
        }

        public LoggingEventArgs(LoggingEvent evt)
        {
            Event = evt;
        }

        #endregion
    }
}
