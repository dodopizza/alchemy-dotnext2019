using System;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Domain
{
    public class NetworkMixChecker : IMixChecker
    {
        public async Task<CheckResult> Check(Guid firstElementId, Guid secondElementId)
        {
            var checkRequest = new CheckRequest
            {
                FirstId = firstElementId.ToString(),
                SecondId = secondElementId.ToString()
            };
            

            //todo: try/catch
            using (var request = CreateApiPostRequest(
                Constants.ApiUrl + "api/workbench", checkRequest))
            {
                request.timeout = Constants.RpcTimeoutSeconds;
                
                await request.SendWebRequest();

                var intermediateResult = JsonUtility.FromJson<InternalCheckResult>(request.downloadHandler.text);
                return new CheckResult
                {
                    IsSuccess = intermediateResult.isSuccess,
                    CreatedElementId = Guid.Parse(intermediateResult.createdElementId),
                    Scores = intermediateResult.scores
                };
            }
        }
        
        public UnityWebRequest CreateApiGetRequest(string actionUrl, object body = null)
        {
            return CreateApiRequest(actionUrl, UnityWebRequest.kHttpVerbGET, body);
        }

        private UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
        {
            return CreateApiRequest(actionUrl, UnityWebRequest.kHttpVerbPOST, body);
        }
        
        UnityWebRequest CreateApiRequest(string url, string method, object body)
        {
            string bodyString = null;
            
            if (body is string s)
            {
                bodyString = s;
            }
            else if (body != null)
            {
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
            request.timeout = 60;
            return request;
        }

        private class CheckRequest
        {
            public string FirstId;

            public string SecondId;
        }

        private class InternalCheckResult
        {
            public bool isSuccess;

            public string createdElementId;

            public int scores;
        }
    }
}