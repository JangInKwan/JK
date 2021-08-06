using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Blazor.Data
{
    /// <summary>
    /// Dialog Data
    /// </summary>
    public class DialogData
    {
        /// <summary>
        /// Dialog 메세지 내용
        /// </summary>
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// 확인 버튼 Text
        /// </summary>
        public string TextOK { get; set; } = "확인";
        /// <summary>
        /// 취소 버튼 Text
        /// </summary>
        public string TextCancel { get; set; } = "닫기";

    }

    /// <summary>
    /// Dialog Type
    /// </summary>
    public enum DialogType
    {
        Alert,
        Confirm
    }
}
