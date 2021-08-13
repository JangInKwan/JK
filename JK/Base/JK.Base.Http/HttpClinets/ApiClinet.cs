using JK.Base.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace JK.Base.HttpClinets
{
    /// <summary>
    /// API 요청
    /// </summary>
    /// <typeparam name="Req">Request Object Type</typeparam>
    /// <typeparam name="Res">Response Object Type</typeparam>
    public class ApiClinet<Req, Res> 
    {
        // Http Rest Api Helper 
        private HttpHelper _httpHelper = new HttpHelper();

        /// <summary>
        /// URL Base Address
        /// </summary>
        public string BaseAddress => _httpHelper.BaseAddress;
        /// <summary>
        /// Content Type
        /// </summary>
        public ContentTypeEnum ContentType => _httpHelper.ContentType;
        /// <summary>
        /// Encoding Type
        /// </summary>
        public Encoding EncodingType => _httpHelper.EncodingType;
        /// <summary>
        /// Response Data
        /// </summary>
        public Res Response { get; private set; } = default(Res);
        /// <summary>
        /// Response String Data
        /// </summary>
        public string ResponseString { get; private set; } = string.Empty;

        /// <summary>
        /// initialization
        /// </summary>
        private void Init()
        {
            try
            {
                Response = default(Res);
                ResponseString = string.Empty;
            }
            catch (Exception e)
            {
                Log.ei(e, "ApiClinet Release Fail");
            }
        }

        /// <summary>
        /// Request Get 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> GetAsync(string path, Req req)
        {
            try
            {
                Init();

                string queryString = string.Empty;
                var data = string.Empty;

                if (req.GetType() == typeof(string))
                {
                    data = req.ToString();
                }
                else
                {
                    var properties = from p in req.GetType().GetProperties()
                                     where p.GetValue(req, null) != null
                                     select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(req, null).ToString());

                    queryString = string.Join("&", properties.ToArray());
                }

                switch (ContentType)
                {
                    case ContentTypeEnum.Xml:
                    case ContentTypeEnum.Html:
                        data = XmlSerializer(req);
                        break;
                    case ContentTypeEnum.Json:
                        data = JsonSerializer(req);
                        break;
                    default:
                        data = req.ToString();
                        break;
                }

                var result = await _httpHelper.GetAsync(path, data);

                if (result == null)
                {
                    return false;
                }

                if (_httpHelper.Response.IsSuccessStatusCode)
                {
                    ResponseString = result;

                    switch (ContentType)
                    {
                        case ContentTypeEnum.Xml:
                        case ContentTypeEnum.Html:
                            Response = XmlDeserialize(result);
                            break;
                        case ContentTypeEnum.Json:
                            Response = JsonDeserialize(result);
                            break;
                        default:
                            data = req.ToString();
                            break;
                    }

                    Log.i($"ApiClinet GetAsync : {result}");
                }
                else
                {
                    Log.i($"ApiClinet GetAsync Fail : {result}");
                }

                return true;
            }
            catch (Exception e)
            {
                Log.ei(e, "ApiClinet GetAsync Exception");

                return false;
            }
        }

        /// <summary>
        /// Request Post 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<bool> PostAsync(string path, Req req)
        {
            try
            {
                Init();

                var data = string.Empty;

                switch (ContentType)
                {
                    case ContentTypeEnum.Xml:
                    case ContentTypeEnum.Html:
                        data = XmlSerializer(req);
                        break;
                    case ContentTypeEnum.Json:
                        data = JsonSerializer(req);
                        break;
                    default:
                        data = req.ToString();
                        break;
                }

                var result = await _httpHelper.PostAsync(path, data);

                if (result == null)
                {
                    return false;
                }

                if (_httpHelper.Response.IsSuccessStatusCode)
                {
                    ResponseString = result;

                    switch (ContentType)
                    {
                        case ContentTypeEnum.Xml:
                        case ContentTypeEnum.Html:
                            Response = XmlDeserialize(result);
                            break;
                        case ContentTypeEnum.Json:
                            Response = JsonDeserialize(result);
                            break;
                        default:
                            data = req.ToString();
                            break;
                    }

                    Log.i($"ApiClinet PostAsync : {result}");
                }
                else
                {
                    Log.i($"ApiClinet PostAsync Fail : {result}");
                }

                return true;
            }
            catch (Exception e)
            {
                Log.ei(e, "ApiClinet PostAsync Exception");

                return false;
            }
        }

        /// <summary>
        /// Xml Serializer
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private string XmlSerializer(Req req)
        {
            string result = string.Empty;
            XmlSerializer reqserializer = new XmlSerializer(new XmlReflectionImporter().ImportTypeMapping(typeof(Req), null, null));

            using (StringWriter writer = new CustomStringWriter(EncodingType))
            {
                reqserializer.Serialize(writer, req);
                result = writer.ToString();
            }

            return result;
        }

        /// <summary>
        /// Xml Deserialize
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private Res XmlDeserialize(string res)
        {
            Res result = default(Res);
            XmlSerializer resSerializer = new XmlSerializer(new XmlReflectionImporter().ImportTypeMapping(typeof(Res), null, null));

            using (TextReader reader = new StringReader(res))
            {

                result = (Res)resSerializer.Deserialize(reader);
            }

            return result;
        }

        /// <summary>
        /// Json Serializer
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private string JsonSerializer(Req req)
        {
            string result = JsonConvert.SerializeObject(this, Formatting.Indented);
            return result;
        }

        /// <summary>
        /// Json Deserialize
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private Res JsonDeserialize(string res)
        {
            Res result = default(Res);
            JsonConvert.PopulateObject(res, result);

            return result;
        }

        private class CustomStringWriter : StringWriter
        {
            private Encoding _encoding = Encoding.UTF8;

            public CustomStringWriter(Encoding encoding)
            {
                if (encoding != null)
                {
                    _encoding = encoding;
                }
            }

            public override Encoding Encoding { get { return _encoding; } }
        }
    }
}
