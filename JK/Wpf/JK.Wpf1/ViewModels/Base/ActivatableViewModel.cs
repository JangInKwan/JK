using System;
using System.Windows.Threading;

namespace JK.Wpf.ViewModels
{
    public abstract class ActivatableViewModel : DispatcherViewModel
    {
        public ActivatableViewModel(Dispatcher dispatcher)
            : base(dispatcher)
        {

        }

        private bool _IsActivated = false;
        /// <summary>
        /// 화면에 Activated 여부
        /// </summary>
        public bool IsActivated
        {
            get { return _IsActivated; }
            set
            {
                if (SetProperty(ref _IsActivated, value))
                    OnIsActivatedChanged();
            }
        }

        public event EventHandler IsActivatedChanged;

        protected virtual void OnIsActivatedChanged()
        {
            IsActivatedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
