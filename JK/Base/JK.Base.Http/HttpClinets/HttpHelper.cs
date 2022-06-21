using JK.Base.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JK.Base.HttpClinets
{
    public class HttpHelper : IDisposable
    {
        private HttpClient _client = null;
        private CancellationTokenSource _cts = null;
        /// <summary>
        /// URL Base Address
        /// </summary>
        public string BaseAddress { get; set; } = string.Empty;
        /// <summary>
        /// Content Type
        /// </summary>
        public ContentTypeEnum ContentType { get; set; } = ContentTypeEnum.Json;
        /// <summary>
        /// Encoding Type
        /// </summary>
        public Encoding EncodingType { get; set; } = Encoding.UTF8;
        /// <summary>
        /// Headers
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// Request Timeout
        /// </summary>
        public int Timeout { get; set; } = 0;
        /// <summary>
        /// Http Response Message
        /// </summary>
        public HttpResponseMessage Response { get; private set; } = null;

        /// <summary>
        /// initialization
        /// </summary>
        private void Init()
        {
            try
            {
                Dispose();

                _client = new HttpClient();
                _client.BaseAddress = new Uri(BaseAddress);
                if (Timeout > 0) _client.Timeout = new TimeSpan(0, 0, Timeout);
                _cts = new CancellationTokenSource();

                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ToHeaderString()));

                foreach (var header in Headers)
                {
                    _client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            catch (Exception e)
            {
                Log.ei(e, "HttpHelper Prepare Fail");
            }
        }

        /// <summary>
        /// Release
        /// </summary>
        public void Dispose()
        {
            try
            {
                _cts?.Cancel();
                _cts = null;
                _client = null;
            }
            catch (Exception e)
            {
                Log.ei(e, "HttpHelper Release Fail");
            }
        }

        /// <summary>
        /// 비동기 API 요청 (Get)
        /// </summary>
        /// <param name="path">URL Sub Path</param>
        /// <param name="data">Request Data</param>
        /// <returns></returns>
        public async Task<string> GetAsync(string path, string queryString)
        {
            try
            {
                Init();

                Response = null;

                string result = string.Empty;

                if (queryString.IndexOf('=') != -1)
                {
                    queryString = "?" + queryString;
                }
                else
                {
                    queryString = "/" + queryString;
                }

                Response = await _client.GetAsync(path + queryString, _cts.Token);

                result = await Response.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception e)
            {
                Log.ei(e, "HttpHelper RequestAsync Fail");
                return null;
            }
        }

        /// <summary>
        /// 비동기 API 요청 (Post)
        /// </summary>
        /// <param name="path">URL Sub Path</param>
        /// <param name="data">Request Data</param>
        /// <returns></returns>
        public async Task<string> PostAsync(string path, string data)
        {
            try
            {
                Init();

                Response = null;
                string result = string.Empty;

                var sc = new StringContent(data, EncodingType, ContentType.ToHeaderString());

                Response = await _client.PostAsync(path, sc, _cts.Token);

                result = await Response.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception e)
            {
                Log.ei(e, "HttpHelper RequestAsync Fail");
                return null;
            }
        }
    }
}
