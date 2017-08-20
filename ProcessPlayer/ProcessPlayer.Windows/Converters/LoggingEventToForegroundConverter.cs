using log4net.Core;
using ProcessPlayer.Windows.Controls;
using ProcessPlayer.Windows.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ProcessPlayer.Windows.Converters
{
    public class LoggingEventToForegroundConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LoggingEvent)
            {
                var evt = (LoggingEvent)value;

                switch (evt.Level.Name)
                {
                    case "DEBUG":
                        return BrushExtensions.DefaultGreen;
                    case "ERROR":
                        return BrushExtensions.DefaultRed;
                    case "FATAL":
                        return BrushExtensions.DefaultGray;
                    case "INFO":
                        return BrushExtensions.DefaultCyan;
                    case "WARN":
                        return BrushExtensions.DefaultCyan;
                    default:
                        return BrushExtensions.DefaultGray;
                }
            }

            return BrushExtensions.DefaultGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
