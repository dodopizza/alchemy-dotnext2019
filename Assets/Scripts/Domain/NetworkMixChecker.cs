using System;
using System.Threading.Tasks;
using Domain.Models;
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
                FirstId = firstElementId.ToString(),
                SecondId = secondElementId.ToString()
            };
            
            //todo: retry
            using (var request = HttpClient.CreateApiPostRequest(Constants.ApiUrl + "api/workbench", checkRequest))
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
                                intermediateResult.scores));
                    }

                    return OperationResult<CheckResult>.Success(CheckResult.Failure());
                }

                return OperationResult<CheckResult>.Failure();
            }
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