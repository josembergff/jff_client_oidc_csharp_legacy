using System.Net;

namespace jff_client_oidc_csharp_legacy.Tools
{
    public static class RequestRest
    {
        public static T1 POST<T1, T2>(string url, string pathUrl, T2 sendObj, string tokenBearer = "", bool sendWWWForm = false)
        {
            return Request<T1, T2>(url, pathUrl, sendObj, "POST", tokenBearer, sendWWWForm);
        }

        public static T1 PUT<T1, T2>(string url, string pathUrl, T2 sendObj, string tokenBearer = "", bool sendWWWForm = false)
        {
            return Request<T1, T2>(url, pathUrl, sendObj, "PUT", tokenBearer, sendWWWForm);
        }

        public static T1 DELETE<T1, T2>(string url, string pathUrl, T2 sendObj, string tokenBearer = "", bool sendWWWForm = false)
        {
            return Request<T1, T2>(url, pathUrl, sendObj, "DELETE", tokenBearer, sendWWWForm);
        }

        public static T1 Request<T1, T2>(string url, string pathUrl, T2 sendObj, string type = "POST", string tokenBearer = "", bool sendWWWForm = false)
        {
            using (WebClient webClient = new WebClient())
            {
                string data = "";
                var baseUrl = url + "/";
                webClient.BaseAddress = baseUrl;
                if (sendWWWForm)
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    if (sendObj != null)
                    {
                        data = QueryConvert.GetQueryString(sendObj);
                    }
                }
                else
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                    if (sendObj != null)
                    {
                        data = JsonConvert.JsonSerializer(sendObj);
                    }
                }
                if (!string.IsNullOrEmpty(tokenBearer))
                {
                    webClient.Headers[HttpRequestHeader.Authorization] = $"Bearer {tokenBearer}";
                }
                webClient.Headers[HttpRequestHeader.Accept] = "*/*";
                var response = webClient.UploadString(pathUrl, type, data);
                var objResult = JsonConvert.JsonDeserializer<T1>(response);

                return objResult;
            }
        }
    }
}
