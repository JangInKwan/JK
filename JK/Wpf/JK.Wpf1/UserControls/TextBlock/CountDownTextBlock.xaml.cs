using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JK.Wpf.UserControls
{
    /// <summary>
    /// 숫자 카운트 다운 에니메이션 TextBlock
    /// </summary>
    public partial class CountDownTextBlock : TextBlock
    {
        public CountDownTextBlock()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CountProperty = DependencyProperty.Register
        (
            name: nameof(Count),
            propertyType: typeof(object),
            ownerType: typeof(CountDownTextBlock),
            typeMetadata: new FrameworkPropertyMetadata(null, ValueProperty_Changed)
        );

        /// <summary>
        /// 카운트
        /// </summary>
        public object Count
        {
            get
            {
                return (object)GetValue(CountProperty);
            }
            set
            {
                SetValue(CountProperty, value);
            }
        }

        /// <summary>
        /// 카운트 변경 이벤트
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ValueProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d == null || e == null)
            {
                return;
            }

            CountDownTextBlock tbCount = d as CountDownTextBlock;

            int count = -1;
            string newValue = e.NewValue?.ToString();

            int.TryParse(newValue, out count);

            if (count < 0 && count > 9)
            {
                return;
            }

            if (count == 0)
            {
                count = 10;
            }

            ThicknessAnimation animation = new ThicknessAnimation();
            animation.From = new Thickness(0, 0, 0, 0);
            animation.To = new Thickness(0, -((tbCount.Height / 11) * count), 0, 0);
            animation.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            tbCount.BeginAnimation(StackPanel.MarginProperty, animation);
        }
    }
}