using System;

namespace Domain
{
    internal interface IElementsBook
    {
        Element[] GetOpenedElements();
        Element OpenAndGetElement(Guid elementId);
        bool ElementOpened(Guid elementId);
    }
}