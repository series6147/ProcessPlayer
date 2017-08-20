using log4net.Appender;
using log4net.Core;
using System;

namespace ProcessPlayer
{
    public class ProcessConsoleAppender : ColoredConsoleAppender
    {
        #region overriden methods

        protected override void Append(LoggingEvent loggingEvent)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(loggingEvent.TimeStamp.ToString("dd.MM.yyyy HH:mm:ss.fff") + " ");

            switch (loggingEvent.Level.Name)
            {
                case "DEBUG":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "ERROR":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "FATAL":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "INFO":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "WARN":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }

            Console.WriteLine(loggingEvent.RenderedMessage);
            Console.ResetColor();
        }

        #endregion
    }
}
