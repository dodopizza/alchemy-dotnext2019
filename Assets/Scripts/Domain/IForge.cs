using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
    internal interface IForge
    {
        void AddElement(Element element);

        void Clear();
        
        Task<OperationResult<MixResult>> GetMixResult();
    }
}