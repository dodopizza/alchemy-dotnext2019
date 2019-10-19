using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IMixChecker
    {
        Task<OperationResult<CheckResult>> Check(Guid firstElementId, Guid secondElementId);
    }
}