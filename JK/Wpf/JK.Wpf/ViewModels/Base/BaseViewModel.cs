using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JK.Wpf.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property 변경 이벤트
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Property 값 변경 및 이벤트 발생(저장된 값이 같은 경우 이벤트 발생하지 않음)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
            {
                if (storage != null && storage.Equals(value))
                    return false;

                storage = value;

                RaisePropertyChanged(propertyName);

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual Task LoadAsync()
        {
            return Task.FromResult(true); ;
        }
    }
}
