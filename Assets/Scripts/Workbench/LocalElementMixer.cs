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
                color = new Color(0.5f, 0.1f, 0.4f)
            };
        }
    }
}