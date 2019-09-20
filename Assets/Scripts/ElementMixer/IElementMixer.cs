using System.Collections.Generic;

namespace ElementMixer
{
    public interface IElementMixer
    {
        ElementData MixElements(IEnumerable<ElementData> elementsData);
    }
}