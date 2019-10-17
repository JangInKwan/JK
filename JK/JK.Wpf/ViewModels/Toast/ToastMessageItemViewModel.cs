using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Wpf.ViewModels
{
    /// <summary>
    /// 토스트 메시지를 나타낸다.
    /// </summary>
    public class ToastMessageItemViewModel : BaseViewModel
    {
        #region Constructors

        /// <summary>
        /// 새로운 인스턴스를 생성한다.
        /// </summary>
        /// <param name="createDateTime">생성일시</param>
        /// <param name="message">메시지</param>
        public ToastMessageItemViewModel(DateTime createDateTime, string message)
        {
            CreatedDateTime = createDateTime;
            Message = message;
        }

        #endregion


        #region Bindable Properties

        /// <summary>
        /// 생성일시.
        /// 바인딩된 경우 UI 스레드에서만 사용해야 한다.
        /// </summary>
        public DateTime CreatedDateTime { get; }

        /// <summary>
        /// 메시지.
        /// 바인딩된 경우 UI 스레드에서만 사용해야 한다.
        /// </summary>
        public string Message { get; }


        DateTime? _deletedDateTime;

        /// <summary>
        /// 삭제된 시간.
        /// 바인딩된 경우 UI 스레드에서만 사용해야 한다.
        /// </summary>
        public DateTime? DeletedDateTime
        {
            get { return _deletedDateTime; }
            set
            {
                if (SetProperty(ref _deletedDateTime, value))
                    RaisePropertyChanged(nameof(IsAboutToDelete));
            }
        }

        /// <summary>
        /// 삭제 예정인 항목인지 여부.
        /// 바인딩된 경우 UI 스레드에서만 사용해야 한다.
        /// </summary>
        public bool IsAboutToDelete => DeletedDateTime != null;

        #endregion
    }
}
