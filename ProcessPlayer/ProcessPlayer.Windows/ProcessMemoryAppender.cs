using log4net.Appender;
using log4net.Core;
using ProcessPlayer.Windows.Interfaces;
using System;

namespace ProcessPlayer.Windows
{
    public class ProcessMemoryAppender : MemoryAppender, ILogEventHandler
    {
        #region properties

        public static ProcessMemoryAppender Current;

        #endregion

        #region constructors

        public ProcessMemoryAppender()
        {
            Current = this;
        }

        #endregion

        #region ILogEventHandler Members

        public event EventHandler<LoggingEventArgs> Appending;

        #endregion

        #region MemoryAppender Members

        protected override void Append(LoggingEvent loggingEvent)
        {
            //base.Append(loggingEvent);

            if (Appending != null)
                Appending(this, new LoggingEventArgs(loggingEvent));
        }

        #endregion
    }
}
