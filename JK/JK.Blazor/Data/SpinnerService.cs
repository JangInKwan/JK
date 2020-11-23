using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Blazor.Data
{
    public class SpinnerService
    {
        /// <summary>
        /// Spinner 상태 변경 이벤트
        /// </summary>
        public Func<bool, Task> SpinnerRequested;

        /// <summary>
        /// Show Spinner 
        /// </summary>
        /// <returns></returns>
        public async void Show()
        {
            await SpinnerRequested?.Invoke(true);
        }

        /// <summary>
        /// Hide Spinner 
        /// </summary>
        public async void Hide()
        {
            await SpinnerRequested?.Invoke(false);
        }

        public async Task Show(Func<Task> action)
        {
            await SpinnerRequested?.Invoke(true);

            await action.Invoke();

            await SpinnerRequested?.Invoke(false);
        }
    }
}
