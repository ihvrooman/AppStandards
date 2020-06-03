using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace AppStandards.Converters
{
    /// <summary>
    /// Attempts to convert to a Boolean value and then returns Visible (true) or Collapsed (false).
    /// If the input value is actually a Visibility enum value, the return is Boolean.
    /// Accepts Boolean, numeric, string, and Visibility (enum) values.
    /// </summary>
    public class BooleanToVisibility : MarkupExtension, IValueConverter
    {
        private static BooleanToVisibility _instance;


        /// <summary>
        /// Default string comparison rules to use when performing text comparisons.
        /// </summary>
        public const StringComparison StringComparison = System.StringComparison.OrdinalIgnoreCase;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool invert;
            bool boolValue;
            double numValue;

            if (value == null)
            {
                // nulls are false
                boolValue = false;
            }
            else if (value.GetType() == typeof(bool))
            {
                // convert boolean or string (easy conversion)
                try { boolValue = System.Convert.ToBoolean(value); }
                catch { boolValue = false; }
            }
            else if (value.GetType() == typeof(string))
            {
                // empty string is "false"
                boolValue = !string.IsNullOrWhiteSpace(value as string);
            }
            else if (value.GetType() == typeof(Visibility))
            {
                // converting visibility to boolean
                boolValue = string.Equals(value.ToString(), Visibility.Visible.ToString(), StringComparison);
                return boolValue;
            }
            else if (double.TryParse(value.ToString(), out numValue))
            {
                // value is a number, convert to boolean
                boolValue = (numValue > 0);
            }
            else if (!bool.TryParse(value.ToString(), out boolValue))
            {
                // last chance: value did not convert from string, bool, or number
                boolValue = false;
            }

            // check for inversion
            if (parameter != null)
            {
                // parse successful and inversion detected
                if (parameter.ToString().Equals("invert", StringComparison) || (bool.TryParse(parameter.ToString(), out invert) && invert))
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }

            // apply without inversion
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("BooleanToVisibility does not support ConvertBack.");
        }

        #endregion

        #region MarkupExtension Members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new BooleanToVisibility());
        }

        #endregion

        #region Static Methods (Custom)

        public static bool Convert(object value)
        {
            var converter = new BooleanToVisibility();
            return (bool)converter.Convert(value, typeof(bool), null, null);
        }

        public static bool Convert(object value, object parameter)
        {
            var converter = new BooleanToVisibility();
            return (bool)converter.Convert(value, typeof(bool), parameter, null);
        }

        #endregion
    }
}

