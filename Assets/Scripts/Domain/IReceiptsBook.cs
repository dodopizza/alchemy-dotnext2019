using System;
using System.Collections.Generic;

namespace Domain
{
    internal interface IReceiptsBook
    {
        IEnumerable<Element> GetOpenedElements();
        
        Element SaveNewReceipt(Guid firstElementId, Guid secondElementId, Guid resultId, bool success);
        
        bool TryGetPreviousResult(Guid firstElementId, Guid secondElementId, out Element element);
    }
}