using ProcessPlayer.Data.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Calculator.Converters
{
    public class InvertBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? !value.ToValue<bool>() : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
