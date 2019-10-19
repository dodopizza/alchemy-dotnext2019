using System;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;
using Utils;

namespace Domain
{
    public class NetworkMixChecker : IMixChecker
    {
        private readonly string _userId;

        public NetworkMixChecker()
        {
            _userId = PlayerPrefs.GetString(Constants.UserIdKey);
        }
        
        public async Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId)
        {
            var checkRequest = new CheckRequest
            {
                FirstElement = firstElementId.ToString(),
                SecondElement = secondElementId.ToString(),
                UserId = _userId
            };
            
            //todo: retry
            using (var request = HttpClient.CreateApiPostRequest(Constants.ApiUrl + "/api/Elements/merge", checkRequest))
            {
                request.timeout = Constants.RpcTimeoutSeconds;
                
                await request.SendWebRequest();

                if (!request.isHttpError && !request.isNetworkError)
                {
                    var intermediateResult = JsonUtility.FromJson<InternalCheckResult>(request.downloadHandler.text);

                    if (intermediateResult.isSuccess)
                    {
                        return OperationResult<CheckResult>.Success(
                            CheckResult.Success(
                                intermediateResult.createdElementId,
                                intermediateResult.spriteName,
                                intermediateResult.name,
                                intermediateResult.scores,
                                intermediateResult.description));
                    }
                    
                    return OperationResult<CheckResult>.Success(CheckResult.Failure());
                }

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

            public string description;

            public int scores;

            public string name;

            public string spriteName;
        }
    }
}