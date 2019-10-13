using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain
{
    internal interface IReceiptsBook
    {
        IEnumerable<Element> GetOpenedElements();
        
        Element SaveNewRecipe(Guid firstElementId, Guid secondElementId, Guid resultId, out bool newlyCreated);

        void SaveAttempt(Guid firstElementId, Guid secondElementId);
        
        bool TryGetPreviousResult(Guid firstElementId, Guid secondElementId, out Element element);
    }
}