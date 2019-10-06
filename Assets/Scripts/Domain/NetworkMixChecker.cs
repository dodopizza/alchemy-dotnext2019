using System;
using System.Threading.Tasks;
using Domain.Models;
using Unity.UNetWeaver;
using UnityEngine;
using Utils;

namespace Domain
{
    public class NetworkMixChecker : IMixChecker
    {
        public async Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId)
        {
            var checkRequest = new CheckRequest
            {
                FirstElement = firstElementId.ToString(),
                SecondElement = secondElementId.ToString(),
                UserId = Guid.NewGuid().ToString()
            };
            
            //todo: retry
            using (var request = HttpClient.CreateApiPostRequest(Constants.ApiUrl + "/api/Elements/test", checkRequest))
            {
                request.timeout = Constants.RpcTimeoutSeconds;
                
                await request.SendWebRequest();

                if (!request.isHttpError && !request.isNetworkError)
                {
                    var intermediateResult = JsonUtility.FromJson<InternalCheckResult>(request.downloadHandler.text);

                    if (intermediateResult.isSuccess)
                    {
                        Debug.Log("Success");
                        return OperationResult<CheckResult>.Success(
                            CheckResult.Success(
                                intermediateResult.createdElementId,
                                intermediateResult.scores));
                    }
                    
                    Debug.Log("Fail");
                    return OperationResult<CheckResult>.Success(CheckResult.Failure());
                }

                Debug.Log($"Server error: {request.error}");
                return OperationResult<CheckResult>.Failure();
            }
        }
 
        private class CheckRequest
        {
            public string FirstElement;

            public string SecondElement;

            public string UserId;
        }

        private class InternalCheckResult
        {
            public bool isSuccess;

            public string createdElementId;

            public int scores;
        }
    }
}