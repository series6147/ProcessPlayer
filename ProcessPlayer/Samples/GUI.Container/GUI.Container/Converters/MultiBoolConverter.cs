using ProcessPlayer.Data.Common;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ViewContainer.Converters
{
    public class MultiBoolConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return null;

            return string.Equals((parameter ?? string.Empty).ToString(), "any", StringComparison.InvariantCultureIgnoreCase)
                ? values.Any(v => v.ToValue<bool>())
                : values.All(v => v.ToValue<bool>());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
