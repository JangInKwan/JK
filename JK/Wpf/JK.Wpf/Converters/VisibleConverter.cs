using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JK.Wpf.Converters
{
    public class VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return System.Windows.Visibility.Collapsed;
            }
            else if (value.GetType() == typeof(bool))
            {
                return (bool)value
                    ? System.Windows.Visibility.Visible
                    : System.Windows.Visibility.Collapsed;
            }
            else if (value.GetType() == typeof(int))
            {
                return System.Windows.Visibility.Visible;
            }
            else if (value.GetType() == typeof(string))
            {
                return string.IsNullOrWhiteSpace((string)value)
                    ? System.Windows.Visibility.Collapsed
                    : System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
