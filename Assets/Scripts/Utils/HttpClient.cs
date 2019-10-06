using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
    public class HttpClient
    {
        public static UnityWebRequest CreateApiGetRequest(string url, object body = null)
        {
            return CreateApiRequest(url, UnityWebRequest.kHttpVerbGET, body);
        }

        public static UnityWebRequest CreateApiPostRequest(string url, object body = null)
        {
            return CreateApiRequest(url, UnityWebRequest.kHttpVerbPOST, body);
        }
        
        private static UnityWebRequest CreateApiRequest(string url, string method, object body)
        {
            string bodyString = null;
            
            if (body is string s)
            {
                bodyString = s;
            }
            else if (body != null)
            {
                // todo: это работает плохо. Поправить
                bodyString = JsonUtility.ToJson(body);
            }

            var request = new UnityWebRequest
            {
                url = url,
                method = method,
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler =
                    new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString))
            };
            
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }
    }
}