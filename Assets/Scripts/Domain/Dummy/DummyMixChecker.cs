using System;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain.Dummy
{
    internal class DummyMixChecker : Interfaces.IMixChecker
    {
        public Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId)
        {
            Debug.Log("Remote call!");
//            if (UnityEngine.Random.Range(0, 2) == 1)
//            {
//                return ReturnResult(CheckResult.Success(
//                    "fd540609-4bb5-4b96-bde0-167ee8e283c6", 
//                    "электричество",
//                    "электричество",
//                    10, 
//                    "description"));
//            }

            return Task.FromResult(OperationResult<CheckResult>.Failure());
            return ReturnResult(CheckResult.Failure());
        }

        private Task<OperationResult<CheckResult>> ReturnResult(CheckResult checkResult)
        {
            return Task.FromResult(OperationResult<CheckResult>.Success(checkResult));
        }
    }
}