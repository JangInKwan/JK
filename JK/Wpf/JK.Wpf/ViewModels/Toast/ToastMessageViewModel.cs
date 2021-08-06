using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JK.Wpf.ViewModels
{
    /// <summary>
    /// 토스트 메시지 기능을 제공한다.
    /// </summary>
    public class ToastMessageViewModel : DispatcherViewModel
    {
        /// <summary>
        /// 새로운 인스턴스를 생성한다.
        /// </summary>
        /// <param name="dispatcher">디스패처</param>
        public ToastMessageViewModel(Dispatcher dispatcher)
            : base(dispatcher)
        {

        }

        readonly ObservableCollection<ToastMessageItemViewModel> _internalItems = new ObservableCollection<ToastMessageItemViewModel>();
        ReadOnlyObservableCollection<ToastMessageItemViewModel> _items;

        /// <summary>
        /// 토스트 메시지 목록.
        /// 이 속성은 항상 null 이 아니다.
        /// 바인딩된 경우 UI 스레드에서만 사용해야 한다.
        /// </summary>
        public ReadOnlyObservableCollection<ToastMessageItemViewModel> Items
        {
            get { return _items ?? (_items = new ReadOnlyObservableCollection<ToastMessageItemViewModel>(_internalItems)); }
        }

        /// <summary>
        /// 메시지를 추가한다.
        /// UI 스레드에서만 사용해야 한다.
        /// </summary>
        /// <param name="message">메시지</param>
        public void Add(string message)
        {
            _internalItems.Add(new ToastMessageItemViewModel(DateTime.Now, message));
            DeleteMessageDelayInvoker();
        }

        /// <summary>
        /// 토스트 메시지를 삭제한다.
        /// UI 스레드에서만 사용해야 한다.
        /// </summary>
        /// <param name="item">삭제할 메시지</param>
        public void Delete(ToastMessageItemViewModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _internalItems.Remove(item);
        }

        /// <summary>
        /// 메시지를 감춰야 하는지 여부를 반환한다.
        /// </summary>
        /// <param name="now">현재 시간</param>
        /// <param name="dateTime">메시지 생성 시간</param>
        /// <returns>메시지를 감춰야 하는지 여부</returns>
        private static bool IsHiddenRequired(DateTime now, DateTime dateTime)
        {
            return (now - dateTime).TotalSeconds > 3d;
        }

        /// <summary>
        /// 메시지를 삭제해야 하는지 여부를 반환한다.
        /// </summary>
        /// <remarks>
        /// 삭제 애니메이션이 표시되도록 일정 시간 이후에 삭제한다.
        /// </remarks>
        /// <param name="now">현재 시간</param>
        /// <param name="dateTime">메시지 생성 시간</param>
        /// <returns>메시지를 삭제해야 하는지 여부</returns>
        private static bool IsDeleteRequired(DateTime now, DateTime dateTime)
        {
            return (now - dateTime).TotalSeconds > 2d;
        }

        /// <summary>
        /// 삭제되어야 할 메시지를 삭제한다.
        /// UI 스레드에서만 사용해야 한다.
        /// </summary>
        private void DeleteMessageIfRequired()
        {
            var now = DateTime.Now;

            foreach (var item in Items.ToList())
            {
                if (item.DeletedDateTime != null)
                {
                    if (IsDeleteRequired(now, item.DeletedDateTime.Value))
                        _internalItems.Remove(item);
                }
                else
                {
                    if (IsHiddenRequired(now, item.CreatedDateTime))
                        item.DeletedDateTime = now;
                }
            }

            if (Items.Any())
                DeleteMessageDelayInvoker();
        }

        /// <summary>
        /// 토스트 메시지를 삭제하는 지연 호출자.
        /// UI 스레드에서만 사용해야 한다.
        /// </summary>
        private void DeleteMessageDelayInvoker()
        {
            Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(1d));

                DeleteMessageIfRequired();
            });
        }
    }
}
