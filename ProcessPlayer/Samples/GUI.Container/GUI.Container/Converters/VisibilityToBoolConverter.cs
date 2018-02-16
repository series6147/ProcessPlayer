using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ViewContainer.Converters
{
    public class VisibilityToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
