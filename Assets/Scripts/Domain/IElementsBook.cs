using System;
using System.Collections.Generic;

namespace Domain
{
    internal interface IElementsBook
    {
        IEnumerable<Element> GetOpenedElements();
        
        bool TryOpenElement(Guid elementId, out Element element);
    }
}