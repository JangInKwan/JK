using JK.Wpf.UserControls.Media;
using System;
using System.Globalization;
using System.Windows.Data;

namespace JK.Wpf.Converters
{
    public class MediaKindToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return System.Windows.Visibility.Collapsed;
            }
            else if (value.GetType() == typeof(MediaKindEnum))
            {
                MediaKindEnum val = (MediaKindEnum)value;
                MediaKindEnum pram = (MediaKindEnum)System.Convert.ToInt32(parameter);

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
