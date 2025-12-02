using JK.Base.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JK.Base.HttpClinets
{
    /// <summary>
    /// 지정된 URL에서 로컬 대상으로 파일을 비동기적으로 다운로드하는 기능을 제공하며,
    /// 진행률 보고 및 취소 기능을 지원합니다.
    /// </summary>
    /// <remarks>DownloadClient는 HTTP를 통한 파일 다운로드를 활성화하고, 작업 중 진행률 이벤트를 발생시키며
    /// 토큰을 통한 취소 기능을 지원합니다. 클라이언트는 대상 파일이 이미 존재하는 경우 덮어씁니다.
    /// 삭제 후에는 인스턴스를 사용해서는 안 됩니다. 이 클래스는 스레드로부터 안전하지 않으므로 각 인스턴스는
    /// 단일 스레드에서 사용해야 합니다.</remarks>
    public class DownloadClient : IDisposable
    {
        private readonly string _downloadUrl;
        private readonly string _destinationFilePath;
        private readonly CancellationToken? _cancellationToken;

        private HttpClient _httpClient;

        public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

        public event ProgressChangedHandler ProgressChanged;

        /// <summary>
        /// 지정된 URL에서 지정된 대상 경로로 파일을 다운로드하기 위해 DownloadClient 클래스의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <remarks>취소 토큰이 제공되고 취소가 요청되면 다운로드 작업이
        /// 중단됩니다. 대상 경로가 쓰기 가능하고 애플리케이션에 충분한 권한이 있는지
        /// 확인하십시오.</remarks>
        /// <param name="downloadUrl">다운로드할 파일의 URL입니다. null이거나 비어 있을 수 없습니다.</param>
        /// <param name="destinationFilePath">다운로드된 파일이 저장될 전체 경로입니다. null이거나 비어 있을 수 없습니다.</param>
        /// <param name="cancellationToken">다운로드 작업을 취소하는 데 사용할 수 있는 선택적 취소 토큰입니다.</param>
        public DownloadClient(string downloadUrl, string destinationFilePath, CancellationToken? cancellationToken = null)
        {
            _downloadUrl = downloadUrl;
            _destinationFilePath = destinationFilePath;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// 구성된 URL에서 비동기 다운로드 작업을 시작합니다.
        /// </summary>
        /// <remarks>다운로드 작업은 1일의 확장된 시간 제한을 사용합니다. 이 메서드는
        /// 다운로드가 완료되거나 오류가 발생할 때까지 반환하지 않습니다. 이 메서드는 다운로드가
        /// 완료된 후 계속 진행하기 위해 대기해야 합니다.</remarks>
        /// <returns>비동기 다운로드 작업을 나타내는 작업입니다.</returns>
        public async Task StartDownload()
        {
            _httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };

            using (var response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                await DownloadFileFromHttpResponseMessage(response);
        }

        /// <summary>
        /// 지정된 HTTP 응답 메시지에서 파일 콘텐츠를 다운로드하고 데이터 스트림을 비동기적으로 처리합니다.
        /// </summary>
        /// <remarks>HTTP 응답이 성공을 나타내지 않으면 이 메서드는 예외를 발생시킵니다.
        /// 호출자는 이 메서드를 호출하기 전에 응답에 예상 파일 콘텐츠가 포함되어 있는지 확인해야 합니다.
        /// </remarks>
        /// <param name="response">다운로드할 파일 콘텐츠가 포함된 HTTP 응답 메시지입니다. 응답은 성공 상태 코드를 나타내야 합니다.
        /// </param>
        /// <returns>비동기 다운로드 및 처리 작업을 나타내는 작업입니다.</returns>
        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength;

            using (var contentStream = await response.Content.ReadAsStreamAsync())
                await ProcessContentStream(totalBytes, contentStream);
        }

        /// <summary>
        /// 지정된 콘텐츠 스트림에서 데이터를 읽고 대상 파일에 쓰며, 주기적으로
        /// 다운로드 진행 상황을 보고합니다.
        /// </summary>
        /// <remarks>진행 상황은 읽기 작업 10회마다, 그리고 파일이 완전히 작성된 후 한 번 보고됩니다.
        /// 취소 토큰이 제공된 경우, 읽는 중에 작업을 취소할 수 있습니다. 대상
        /// 파일이 이미 존재하는 경우 덮어씁니다.</remarks>
        /// <param name="totalDownloadSize">다운로드의 예상 총 크기(바이트) 또는 알 수 없는 경우 null 입니다. 진행 상황 보고에</param>
        /// <param name="contentStream">다운로드할 콘텐츠가 포함된 스트림입니다. 읽을 수 있어야 하며 콘텐츠의 시작 부분에 위치해야 합니다.</param>
        /// <returns>콘텐츠 스트림을 처리하고 저장하는 비동기 작업을 나타내는 작업입니다.</returns>
        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            var totalBytesRead = 0L;
            var readCount = 0L;
            var buffer = new byte[8192];
            var isMoreToRead = true;

            using (var fileStream = new FileStream(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                do
                {
                    int bytesRead;
                    if (_cancellationToken.HasValue)
                    {
                        bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, _cancellationToken.Value);
                    }
                    else
                    {
                        bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    }

                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        continue;
                    }

                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                    totalBytesRead += bytesRead;
                    readCount += 1;

                    if (readCount % 10 == 0)
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                }
                while (isMoreToRead);

            }

            //the last progress trigger should occur after the file handle has been released or you may get file locked error
            TriggerProgressChanged(totalDownloadSize, totalBytesRead);
        }

        /// <summary>
        /// 진행률 변경 이벤트를 발생시켜 업데이트된 다운로드 크기 및 읽은 바이트 정보를 이벤트 구독자에게 제공합니다.
        /// </summary>
        /// <param name="totalDownloadSize">전체 다운로드 크기(바이트)입니다. null 인 경우 전체 크기를 알 수 없습니다.</param>
        /// <param name="totalBytesRead">다운로드 작업 중 지금까지 읽은 총 바이트 수입니다.</param>
        /// <summary>
        private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        {
            if (ProgressChanged == null)
                return;

            double? progressPercentage = null;
            if (totalDownloadSize.HasValue)
                progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

            ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
        }

        /// <summary>
        /// 현재 인스턴스에서 사용하는 리소스를 해제합니다.
        /// </summary>
        /// <remarks>인스턴스 사용이 끝나면 이 메서드를 호출하여 관리되지 않는 리소스를 해제하고
        /// 기타 정리 작업을 수행합니다. Dispose를 호출한 후에는 인스턴스를 사용해서는 안 됩니다.</remarks>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}