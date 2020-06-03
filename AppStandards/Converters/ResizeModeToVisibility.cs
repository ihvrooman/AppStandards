using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AppStandards.Converters
{
    internal class ResizeModeToVisibility : MarkupExtension, IValueConverter
    {
        private static ResizeModeToVisibility _instance;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var buttonType = (ButtonType)parameter;
            var resizeMode = (ResizeMode)value;

            switch (resizeMode)
            {
                case ResizeMode.NoResize:
                    return Visibility.Collapsed;
                case ResizeMode.CanMinimize:
                    if (buttonType == ButtonType.MinimizeButton)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Collapsed;
                case ResizeMode.CanResize:
                    return Visibility.Visible;
                case ResizeMode.CanResizeWithGrip:
                    return Visibility.Visible;
                default:
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ResizeModeToVisibility does not support ConvertBack.");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_instance == null)
            {
                _instance = new ResizeModeToVisibility();
            }
            return _instance;
        }
    }
}
