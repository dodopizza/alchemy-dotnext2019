using System;

namespace Domain
{
    internal interface IElementsBook
    {
        Element[] GetOpenedElements();
        bool TryOpenElement(Guid elementId, out Element element);
    }
}