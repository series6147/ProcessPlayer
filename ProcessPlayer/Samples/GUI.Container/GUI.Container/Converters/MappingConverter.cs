using ProcessPlayer.Data.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ViewContainer.Converters
{
    public class MappingConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Array)
                return ((Array)parameter).GetValue(value.ToValue<int>());

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
