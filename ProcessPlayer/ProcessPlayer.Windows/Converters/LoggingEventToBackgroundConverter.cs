using log4net.Core;
using ProcessPlayer.Windows.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ProcessPlayer.Windows.Converters
{
    public class LoggingEventToBackgroundConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LoggingEvent)
            {
                var evt = (LoggingEvent)value;

                switch (evt.Level.Name)
                {
                    case "FATAL":
                        return BrushExtensions.DefaultDarkRed;
                    default:
                        return BrushExtensions.DefaultTransparent;
                }
            }

            return BrushExtensions.DefaultTransparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
