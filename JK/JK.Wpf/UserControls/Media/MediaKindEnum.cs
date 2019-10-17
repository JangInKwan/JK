using JK.Base.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Wpf.UserControls.Media
{
    /// <summary>
    /// 미디어 종류 Enum
    /// </summary>
    public enum MediaKindEnum
    {
        None = 0,
        Image = 1,
        Video = 2,
    }

    public static class MediaKindEnumExtensions
    {
        private static List<string> _videoFileExtension = new List<string> { ".MP4", ".MPEG", ".MPG", ".WMV", ".AVI" };
        private static List<string> _imageFileExtension = new List<string> { ".JPEG", ".JPG", ".PNG", ".GIF" };
        /// <summary>
        /// 미디어 파일 확장자로 미디어 타입 체크
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static MediaKindEnum CheckMediaFileKindToEnum(this MediaKindEnum value, string extension)
        {
            try
            {
                var isVideo = _videoFileExtension.FindIndex(a => a == extension.ToUpper());

                if (isVideo > -1)
                {
                    return MediaKindEnum.Video;
                }

                var isImage = _imageFileExtension.FindIndex(a => a == extension.ToUpper());

                if (isImage > -1)
                {
                    return MediaKindEnum.Image;
                }

                return MediaKindEnum.None;
            }
            catch (Exception e)
            {
                Log.ei("MediaKindEnum CheckFileMediaKindEnum Exception", e);
                return MediaKindEnum.None;
            }

        }
    }
}