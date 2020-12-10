using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys
{
    public static class HttpRequestHelper
    {
        /// <summary>
        /// Http Get Request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(this string url)
        {
            string strGetResponse;
            try
            {
                var getRequest = CreateHttpRequest(url, "GET");
                var getResponse = getRequest.GetResponse() as HttpWebResponse;
                strGetResponse = GetHttpResponse(getResponse, "GET");
            }
            catch (Exception ex)
            {
                strGetResponse = ex.Message;
            }
            return strGetResponse;
        }

        /// <summary>
        /// Http Get Request Async
        /// </summary>
        /// <param name="url"></param>
        public static async void HttpGetAsync(this string url)
        {
            string strGetResponse;
            try
            {
                var getRequest = CreateHttpRequest(url, "GET");
                var getResponse = await getRequest.GetResponseAsync() as HttpWebResponse;
                strGetResponse = GetHttpResponse(getResponse, "GET");
            }
            catch (Exception ex)
            {
                strGetResponse = ex.Message;
            }
            // return strGetResponse;
            Console.WriteLine("reslut:" + strGetResponse);
        }

        /// <summary>
        /// Http Post Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string HttpPost(this string url, string postData, string contentType = null)
        {
            int random = new Random().Next(0, 10000);
            typeof(HttpRequestHelper).LogDebug("-->[" + random + "]StartWith : " + url + " [ data:" + postData + "]");
            string strPostReponse;
            HttpWebResponse postResponse = null;
            HttpWebRequest postRequest = null;
            try
            {
                postRequest = CreateHttpRequest(url, "POST", postData, contentType);
                postResponse = postRequest.GetResponse() as HttpWebResponse;
                strPostReponse = GetHttpResponse(postResponse, "POST");
            }
            catch (Exception ex)
            {
                strPostReponse = ex.Message;
            }
            finally
            {
                postRequest?.Abort();
                postResponse?.Close();
            }
            typeof(HttpRequestHelper).LogDebug("-->[" + random + "]EndWith : " + strPostReponse);
            return strPostReponse;
        }

        /// <summary>
        /// Http Post Request Async
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        public static async Task<string> HttpPostAsync(this string url, string postData, string contentType = null)
        {
            int random = new Random().Next(0, 10000);
            typeof(HttpRequestHelper).LogDebug("-->[" + random + "]StartWith : " + url + " [ data:" + postData + "]");
            string strPostReponse;
            try
            {
                var postRequest = CreatePostHttpWebRequest(url, postData, contentType);
                var postResponse = await postRequest.GetResponseAsync() as HttpWebResponse;
                strPostReponse = GetHttpResponse(postResponse, "POST");
            }
            catch (Exception ex)
            {
                strPostReponse = ex.Message;
            }
            typeof(HttpRequestHelper).LogDebug("-->[" + random + "]EndWith : " + strPostReponse);

            return strPostReponse;
        }

        private static HttpWebRequest CreateHttpRequest(string url, string requestType, params object[] strJson)
        {
            HttpWebRequest request = null;
            const string get = "GET";
            const string post = "POST";
            if (string.Equals(requestType, get, StringComparison.OrdinalIgnoreCase))
            {
                request = CreateGetHttpWebRequest(url);
            }
            if (string.Equals(requestType, post, StringComparison.OrdinalIgnoreCase))
            {
                request = CreatePostHttpWebRequest(url, strJson[0].ToString());
            }
            return request;
        }

        private static HttpWebRequest CreateGetHttpWebRequest(string url)
        {
            var getRequest = (HttpWebRequest)WebRequest.Create(url);
            getRequest.Method = "GET";
            getRequest.Timeout = 5000;
            getRequest.ContentType = "text/html;charset=UTF-8";
            getRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return getRequest;
        }

        private static HttpWebRequest CreatePostHttpWebRequest(string url, string postData, string contentType = null)
        {
            GC.Collect();
            //ServicePointManager.DefaultConnectionLimit = 10;
            var postRequest = (HttpWebRequest)WebRequest.Create(url);
            postRequest.ServicePoint.Expect100Continue = false;
            postRequest.KeepAlive = false;
            postRequest.Timeout = 5000;
            postRequest.Method = "POST";
            postRequest.ContentType = contentType ?? "application/json";//"application/x-www-form-urlencoded";
            postRequest.AllowWriteStreamBuffering = false;
            byte[] data = Encoding.UTF8.GetBytes(postData);
            postRequest.ContentLength = data.Length;
            Stream newStream = postRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            //StreamWriter writer = new StreamWriter(postRequest.GetRequestStream(), Encoding.ASCII);
            //writer.Write(postData);
            //writer.Flush();
            return postRequest;
        }

        private static string GetHttpResponse(HttpWebResponse response, string requestType)
        {
            const string post = "POST";
            string encoding = "UTF-8";
            if (string.Equals(requestType, post, StringComparison.OrdinalIgnoreCase))
            {
                encoding = response.ContentEncoding;
                if (encoding.Length < 1)
                {
                    encoding = "UTF-8";
                }
            }
            string responseResult;
            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.GetEncoding(encoding)))
            {
                responseResult = reader.ReadToEnd();
            }
            return responseResult;
        }


        public static string SendRequest(this string urlStr, string dataStr, string authKey = null, string contentType = null)
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            string respResult = null;
            HttpWebRequest req = null;
            typeof(HttpRequestHelper).LogDebug($"Statr-[{urlStr}-{authKey}] WITH:[{dataStr}]");
            try
            {
                GC.Collect();
                Uri uri = new Uri(urlStr);
                ServicePoint spSite = ServicePointManager.FindServicePoint(uri);
                spSite.ConnectionLimit = 50;
                Stream streamSend = null;
                req = (HttpWebRequest)WebRequest.Create(urlStr);
                req.Method = "POST";
                if (!string.IsNullOrEmpty(authKey))
                {
                    var base64Str = authKey.EncodeBase64();
                    SetHeaderValue(req.Headers, "Authorization", "Bearer " + base64Str);
                    typeof(HttpRequestHelper).LogDebug($"[Authorization]-[{authKey}]-[Bearer {base64Str}]");
                }
                req.ContentType = contentType ?? "application/json";
                req.Accept = "*/*";
                req.Timeout = 5 * 60 * 60 * 1000;
                req.UserAgent = "Mozilla-Firefox";
                //这个在Post的时候，一定要加上，如果服务器返回错误，他还会继续再去请求，不会使用之前的错误数据，做返回数据
                req.ServicePoint.Expect100Continue = false;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                req.CachePolicy = noCachePolicy;
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(dataStr);
                    req.ContentLength = data.Length;
                    streamSend = req.GetRequestStream();
                    streamSend.Write(data, 0, data.Length);
                    streamSend.Close();
                }
                catch (WebException wex)
                {
                    typeof(HttpRequestHelper).LogDebug("[Error]-[WebException]-" + wex + ",wex.Status=" + wex.Status);
                    streamSend?.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    typeof(HttpRequestHelper).LogDebug("[Error]-[GetRequestStream]-" + ex);
                    streamSend?.Close();
                    return null;
                }

                try
                {
                    respResult = GetResp(req);
                    typeof(HttpRequestHelper).LogDebug($"End- WITH:[{respResult}]");

                }
                catch (WebException httpwex)
                {
                    //typeof(HttpRequestHelper).LogDebug("[Error]-[WebException]-" + httpwex + ",wex.Status=" + httpwex.Status);
                    respResult = GetRespStr(httpwex.Response);
                    typeof(HttpRequestHelper).LogDebug($"End- WITH:[{respResult}]");
                    return respResult;
                }
                catch (Exception httpex)
                {
                    typeof(HttpRequestHelper).LogDebug("[Error]-[SendRequest]-" + httpex);
                    return respResult;
                }
                finally
                {
                    streamSend.Close();
                }
            }
            catch (Exception eee)
            {
                typeof(HttpRequestHelper).LogDebug("[Error]" + eee + eee.Source + eee.StackTrace);
            }
            finally
            {
                req?.Abort();
                req = null;
            }
            return respResult;
        }
        public static async Task<string> SendRequestAsync(this string urlStr, string dataStr, string authKey = null, string contentType = null)
        {
            string respResult = null;
            typeof(HttpRequestHelper).LogDebug($"Statr-[{urlStr}-{authKey}] WITH:[{dataStr}]");
            try
            {
                Stream streamSend = null;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(urlStr);
                req.Method = "POST";
                if (!string.IsNullOrEmpty(authKey))
                {
                    var base64Str = authKey.EncodeBase64();
                    SetHeaderValue(req.Headers, "Authorization", "Bearer " + base64Str);
                }
                req.ContentType = contentType ?? "application/json";
                req.Accept = "*/*";
                req.Timeout = 2000;
                req.UserAgent = "Mozilla-Firefox-Spider(Wenanry)";
                //这个在Post的时候，一定要加上，如果服务器返回错误，他还会继续再去请求，不会使用之前的错误数据，做返回数据
                req.ServicePoint.Expect100Continue = false;
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                req.CachePolicy = noCachePolicy;
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(dataStr);
                    req.ContentLength = data.Length;
                    streamSend = req.GetRequestStream();
                    streamSend.Write(data, 0, data.Length);
                    streamSend.Close();
                }
                catch (WebException wex)
                {
                    typeof(HttpRequestHelper).LogDebug("WebException=" + wex + ",wex.Status=" + wex.Status);
                    streamSend?.Close();
                    return null;
                }
                catch (Exception ex)
                {
                    typeof(HttpRequestHelper).LogDebug("GetRequestStream=" + ex);
                    streamSend?.Close();
                    return null;
                }
                try
                {
                    respResult = await GetRespAsync(req);
                    typeof(HttpRequestHelper).LogDebug($"End- WITH:[{respResult}]");
                }
                catch (WebException httpwex)
                {
                    typeof(HttpRequestHelper).LogDebug("WebException=" + httpwex + ",wex.Status=" + httpwex.Status);
                    streamSend.Close();
                    return null;
                }
                catch (Exception httpex)
                {
                    typeof(HttpRequestHelper).LogDebug("SendRequest=" + httpex);
                    return respResult;
                }
            }
            catch (Exception eee)
            {
                typeof(HttpRequestHelper).LogDebug("eee=" + eee + eee.Source + eee.StackTrace);
            }
            return respResult;
        }

        private static async Task<string> GetRespAsync(HttpWebRequest req)
        {
            Stream streamRequest = null;
            try
            {
                string respResult = "";
                streamRequest = (await req.GetResponseAsync()).GetResponseStream();
                if (streamRequest != null)
                    using (StreamReader reader = new StreamReader(streamRequest))
                    {
                        respResult = reader.ReadToEnd();
                    }
                return respResult;
            }
            finally
            {
                streamRequest?.Close();
            }
        }
        private static string GetResp(HttpWebRequest req)
        {
            string respResult = GetRespStr(req.GetResponse());
            return respResult;
        }
        private static string GetRespStr(WebResponse rsp)
        {
            Stream streamRequest = null;
            try
            {
                string respResult = "";
                streamRequest = rsp.GetResponseStream();
                if (streamRequest != null)
                    using (StreamReader reader = new StreamReader(streamRequest))
                    {
                        respResult = reader.ReadToEnd();
                    }
                return respResult;
            }
            finally
            {
                streamRequest?.Close();
            }
        }

        private static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                if (property.GetValue(header, null) is NameValueCollection collection)
                    collection[name] = value;
            }
        }

    }
}
