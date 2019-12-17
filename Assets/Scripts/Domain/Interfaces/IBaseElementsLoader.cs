using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBaseElementsLoader
    {
        Task<OperationResult<IEnumerable<Element>>> GetBaseElements();
    }
}