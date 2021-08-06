using JK.Base.Logs;
using JK.Wpf.Media.ViewModels.Media;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JK.Wpf.Media.UserControls.Media
{
    /// <summary>
    /// MediaPlayer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MediaPlayer : UserControl
    {
        public MediaPlayer()
        {
            InitializeComponent();
        }
        public MediaPlayerViewModel? ViewModel => DataContext as MediaPlayerViewModel;

        /// <summary>
        /// 동영상 재생이 완료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            Log.i("meVideo MediaEnded");
        }

        /// <summary>
        /// 동영상 재생 실패 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void meVideo_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Log.i("meVideo MediaFailed");
        }

        /// <summary>
        /// 미디어 최초 오픈 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            Log.i("meVideo MediaOpened");
        }
    }
}