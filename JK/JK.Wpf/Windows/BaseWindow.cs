using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace JK.Wpf.Windows
{
    public class BaseWindow : Window
    {
        public BaseWindow(bool UseWindowMove, bool UseWindowStateChange)
        {
#if DEBUG
            MouseDownDragMove();
            MouseDoubleClickWindowStateChange();
#else
            if (UseWindowMove)
                MouseDownDragMove();

            if (UseWindowStateChange)
                MouseDoubleClickWindowStateChange();
#endif
        }

        #region 윈도우 설정
        /// <summary>
        ///  마우스 다운시 윈도우 이동
        /// </summary>
        private void MouseDownDragMove()
        {
            this.MouseDown += (s, mbe) =>
            {
                if (mbe.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
        }

        /// <summary>
        /// 더블 클릭시 윈도우 최대(최소)화
        /// </summary>
        private void MouseDoubleClickWindowStateChange()
        {
            this.MouseDoubleClick += (s, mbe) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
        }


        #endregion
    }
}
