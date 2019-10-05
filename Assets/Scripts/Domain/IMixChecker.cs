using System;
using System.Threading.Tasks;

namespace Domain
{
    public interface IMixChecker
    {
        Task<CheckResult> Check(Guid firstElementId, Guid secondElementId);
    }
}