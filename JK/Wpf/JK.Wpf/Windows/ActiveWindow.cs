using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace JK.Wpf.Windows
{
    public class ActiveWindow : BaseWindow
    {

        public ActiveWindow() : base(true, true)
        {
            HideMouseSursor();
            WindowActive();
        }

        public ActiveWindow(bool UseHideMouseSursor, bool UseWindowActive, bool UseWindowMove, bool UseMaximized) : base(UseWindowMove, UseMaximized)
        {
            if (UseHideMouseSursor)
                HideMouseSursor();

            if (UseWindowActive)
                WindowActive();
        }

        #region HideMouseSursor (마우스 커서 3초후 사라지게)
        private Timer _mouseSursorTimer = new Timer()
        {
            AutoReset = false,
            Interval = 3 * 1000,
        };

        /// <summary>
        /// 마우스 커서가 멈추면 3초후 커서가 사라짐
        /// </summary>
        private void HideMouseSursor()
        {
            _mouseSursorTimer.Elapsed += (s, e) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (Mouse.OverrideCursor == null)
                    {
                        Mouse.OverrideCursor = Cursors.None;
                    }
                }));
            };

            this.MouseMove += (s, e) =>
            {
                _mouseSursorTimer.Stop();

                if (Mouse.OverrideCursor == Cursors.None)
                {
                    Mouse.OverrideCursor = null;
                }

                _mouseSursorTimer.Start();
            };

            Mouse.OverrideCursor = Cursors.None;
        }
        #endregion HideMouseSursor

        #region 화면보호기/절전모드 방지하기
        [DllImport("CoreDll.dll")]
        public static extern void SystemIdleTimerReset();

        private void WindowActive()
        {
            Timer timer = new Timer(10 * 1000);

            timer.Elapsed += (s, e) =>
            {
                SystemIdleTimerReset();
            };
        }

        #endregion
    }
}
