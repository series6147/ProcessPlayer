using System.Windows.Media;

namespace ProcessPlayer.Windows.Extensions
{
    public static class BrushExtensions
    {
        #region private properties

        private static SolidColorBrush _defaultCyan;
        private static SolidColorBrush _defaultDarkRed;
        private static SolidColorBrush _defaultGray;
        private static SolidColorBrush _defaultGreen;
        private static SolidColorBrush _defaultRed;
        private static SolidColorBrush _defaultTransparent;

        #endregion

        #region properties

        public static SolidColorBrush DefaultCyan
        {
            get
            {
                if (_defaultCyan == null)
                    _defaultCyan = new SolidColorBrush(Colors.Cyan);
                return _defaultCyan;
            }
        }

        public static SolidColorBrush DefaultDarkRed
        {
            get
            {
                if (_defaultDarkRed == null)
                    _defaultDarkRed = new SolidColorBrush(Colors.DarkRed);
                return _defaultDarkRed;
            }
        }

        public static SolidColorBrush DefaultGray
        {
            get
            {
                if (_defaultGray == null)
                    _defaultGray = new SolidColorBrush(Colors.Gray);
                return _defaultGray;
            }
        }

        public static SolidColorBrush DefaultGreen
        {
            get
            {
                if (_defaultGreen == null)
                    _defaultGreen = new SolidColorBrush(Colors.Green);
                return _defaultGreen;
            }
        }

        public static SolidColorBrush DefaultRed
        {
            get
            {
                if (_defaultRed == null)
                    _defaultRed = new SolidColorBrush(Colors.Red);
                return _defaultRed;
            }
        }

        public static SolidColorBrush DefaultTransparent
        {
            get
            {
                if (_defaultTransparent == null)
                    _defaultTransparent = new SolidColorBrush(Colors.Transparent);
                return _defaultTransparent;
            }
        }

        #endregion
    }
}
