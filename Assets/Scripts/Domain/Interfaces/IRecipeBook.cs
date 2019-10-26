using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    internal interface IRecipeBook
    {
        bool CheckAndSaveNewRecipe(Guid firstElementId, Guid secondElementId, Element createdElement);

        void SaveAttempt(Guid firstElementId, Guid secondElementId);
        
        bool TryGetPreviousResult(Guid firstElementId, Guid secondElementId, out Element element);

        Task<OperationResult<IEnumerable<Element>>> LoadInitialElements();
    }
}