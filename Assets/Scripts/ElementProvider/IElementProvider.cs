using System.Collections.Generic;

namespace ElementProvider
{
    public interface IElementProvider
    {
        List<ElementData> GetElements();
    }
}