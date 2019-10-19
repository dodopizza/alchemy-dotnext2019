using System;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain
{
    internal class DummyMixChecker : IMixChecker
    {
        public Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId)
        {
            Debug.Log("Remote call!");
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                return ReturnResult(CheckResult.Success("959ba1ca-7239-4a42-8f30-b5de84396faa", 10, "description"));
            }

            return ReturnResult(CheckResult.Failure());
        }

        private Task<OperationResult<CheckResult>> ReturnResult(CheckResult checkResult)
        {
            return Task.FromResult(OperationResult<CheckResult>.Success(checkResult));
        }
    }
}