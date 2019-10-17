using JK.Base.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JK.Base.HttpClinets
{
    /// <summary>
    /// 파일 다운로드
    /// </summary>
    public class DownloadClient
    {
        /// <summary>
        /// 비동기 파일 다운로드
        /// </summary>
        /// <param name="url"></param>
        /// <param name="downloadPath"></param>
        /// <param name="DownloadProgressChanged"></param>
        /// <param name="DownloadFileCompleted"></param>
        /// <returns></returns>
        public async Task<bool> DownloadAsync(Uri url, string downloadPath, DownloadProgressChangedEventHandler DownloadProgressChanged = null, AsyncCompletedEventHandler DownloadFileCompleted = null)
        {
            try
            {
                using (WebClient oWebClient = new WebClient())
                {
                    Log.i($"Download : {url}, {downloadPath}");

                    if (DownloadProgressChanged != null)
                    {
                        oWebClient.DownloadProgressChanged += DownloadProgressChanged;
                    }

                    await oWebClient.DownloadFileTaskAsync(url, downloadPath);

                    if (DownloadProgressChanged != null)
                    {
                        oWebClient.DownloadFileCompleted += (s, e) => { oWebClient.DownloadProgressChanged -= DownloadProgressChanged; };
                    }
                    else if (DownloadFileCompleted != null)
                    {
                        oWebClient.DownloadFileCompleted += DownloadFileCompleted;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(downloadPath);

                Log.ei("Download Item Exception", ex);
                return false;
            }
        }
    }
}

