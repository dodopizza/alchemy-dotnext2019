using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElementMixer
{
    public class LocalElementMixer : IElementMixer
    {
        public ElementData MixElements(IEnumerable<ElementData> elementsData)
        {
            if (!elementsData.Any())
            {
                return null;
            }
            
            return new ElementData
            {
                r = 0.5f,
                g = 0.1f,
                b = 0.4f
            };
        }
    }
}