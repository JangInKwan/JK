using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Blazor.Data
{
    /// <summary>
    /// Dialog Service
    /// </summary>
    public class DialogService
    {
        /// <summary>
        /// Dialog Data
        /// </summary>
        public DialogData Data { get; private set; } = new DialogData();

        /// <summary>
        /// Dialog Type
        /// </summary>
        public DialogType DialogType { get; private set; } = DialogType.Alert;

        /// <summary>
        /// Dialog 상태 변경 이벤트
        /// </summary>
        public Action RefreshRequested;

        /// <summary>
        /// Dialog 화면 닫힘
        /// </summary>
        public TaskCompletionSource<bool> Tcs = new TaskCompletionSource<bool>();

        /// <summary>
        /// Alert 생성
        /// </summary>
        /// <param name="data">Dialog Data</param>
        /// <returns></returns>
        public async Task ShowAlert(DialogData data)
        {
            try
            {
                Tcs = new TaskCompletionSource<bool>();

                DialogType = DialogType.Alert;
                Data = data;

                RefreshRequested?.Invoke();

                await Tcs.Task;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Alert 생성
        /// </summary>
        /// <param name="msg">Alert 메세지</param>
        /// <returns></returns>
        public async Task ShowAlert(string msg)
        {
            await ShowAlert(new DialogData { Content = msg });
        }

        /// <summary>
        /// Confirm 생성
        /// </summary>
        /// <param name="data">Dialog Data</param>
        /// <returns></returns>
        public async Task<bool> ShowConfirm(DialogData data)
        {
            try
            {
                Tcs = new TaskCompletionSource<bool>();

                DialogType = DialogType.Confirm;
                Data = data;

                RefreshRequested?.Invoke();
            }
            catch (Exception ex)
            {

            }

            return await Tcs.Task;
        }

        /// <summary>
        /// Confirm 생성
        /// </summary>
        /// <param name="msg">Confirm 메세지</param>
        /// <returns></returns>
        public async Task<bool> ShowConfirm(string msg)
        {
            return await ShowConfirm(new DialogData { Content = msg });
        }
    }
}
