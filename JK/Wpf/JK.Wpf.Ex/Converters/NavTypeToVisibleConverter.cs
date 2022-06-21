using JK.Wpf.Ex.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JK.Wpf.Ex.Converters
{
    public class NavTypeToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return System.Windows.Visibility.Collapsed;
            }
            else if (value.GetType() == typeof(NavType))
            {
                NavType val = (NavType)value;
                NavType pram = (NavType)System.Convert.ToInt32(parameter);

                return val == pram ? System.Windows.Visibility.Visible
                    : System.Windows.Visibility.Collapsed;
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
