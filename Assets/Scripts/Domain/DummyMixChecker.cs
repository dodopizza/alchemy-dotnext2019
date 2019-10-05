using System;
using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain
{
    internal class DummyMixChecker : IMixChecker
    {
        public Task<CheckResult> Check(Guid firstElementId, Guid secondElementId)
        {
            Debug.Log("Remote call!");
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                return Task.FromResult(new CheckResult
                {
                    IsSuccess = true,
                    CreatedElementId = Guid.Parse("959ba1ca-7239-4a42-8f30-b5de84396faa"),
                    Scores = 10
                });
            }

            return Task.FromResult(new CheckResult
            {
                IsSuccess = false
            });
        }
    }
}