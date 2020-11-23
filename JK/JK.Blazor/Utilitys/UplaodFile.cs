using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JK.Blazor.Utilitys
{
    /// <summary>
    /// 파일 업로드
    /// </summary>
    public class UplaodFile
    {
        /// <summary>
        /// 마켓 디폴트 이미지 Path
        /// </summary>
        public const string PathMarketDefaultImage = @"\wwwroot\market\default";
        /// <summary>
        /// 마켓 파일 Path
        /// </summary>
        public const string PathMarketFile = @"\wwwroot\market\files";
        /// <summary>
        /// 마켓 이미지 Path
        /// </summary>
        public const string PathMarketImage = @"\wwwroot\market\images";

        /// <summary>
        /// 마켓 디폴트 이미지 사이즈
        /// </summary>
        public static Size SizeMarketDefaultImage = new Size(800, 500);

        /// <summary>
        /// 업로드 파일 저장
        /// </summary>
        /// <param name="files">파일 리스트</param>
        /// <param name="path">path</param>
        /// <returns></returns>
        public static async Task<bool> Save(List<IFileListEntry> files, string path)
        {
            bool result = false;

            foreach (var file in files)
            {
                result = await Save(file, path);

                if (!result)
                    break;
            }

            return result;
        }

        /// <summary>
        /// 업로드 파일 저장
        /// </summary>
        /// <param name="file">파일</param>
        /// <param name="path">path</param>
        /// <returns></returns>
        public static async Task<bool> Save(IFileListEntry file, string path)
        {
            bool result = false;

            try
            {
                if (file == null || file.Size == 0)
                {
                    return false;
                }

                var filePath = System.IO.Directory.GetCurrentDirectory() + path;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                if (File.Exists(filePath + file.Name))
                {
                    File.Delete(filePath + file.Name);
                }

                using (FileStream fs = System.IO.File.Create(filePath + file.Name))
                {
                    await file.Data.CopyToAsync(fs);
                    fs.Flush();
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// 리사이즈 이미지 저장
        /// </summary>
        /// <param name="file">파일</param>
        /// <param name="path">path</param>
        /// <returns></returns>
        public static async Task<bool> SaveResizeImage(IFileListEntry file, string path, Size size)
        {
            bool result = false;

            try
            {
                if (file == null || file.Size == 0)
                {
                    return false;
                }

                var filePath = System.IO.Directory.GetCurrentDirectory() + path;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                if (File.Exists(filePath + file.Name))
                {
                    File.Delete(filePath + file.Name);
                }

                using (var ms = new MemoryStream())
                {
                    await file.Data.CopyToAsync(ms);

                    using var image = Image.Load(ms.ToArray());
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = size,
                        Mode = ResizeMode.Pad,

                    }));
                    await image.SaveAsync(filePath + file.Name);
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        public static async Task<bool> DeleteDirectoryFile(string path)
        {
            bool result = false;

            try
            {
                var directorPath = System.IO.Directory.GetCurrentDirectory() + path;

                if (Directory.Exists(directorPath))
                {
                    await Task.Factory.StartNew(() =>
                    {
                        Directory.Delete(directorPath, true);
                    });
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public interface IFileListEntry
        {
            DateTime LastModified { get; }
            string Name { get; }
            long Size { get; }
            string Type { get; }
            string RelativePath { get; set; }
            Stream Data { get; }

            event EventHandler OnDataRead;

            Task<IFileListEntry> ToImageFileAsync(string format, int maxWidth, int maxHeight);
        }
    }
}