using System.Collections.Generic;
using UnityEngine;

namespace ElementProvider
{
    public static class ElementProviderFactory
    {
        public static IElementProvider Provider { get; } = new LocalElementProvider();
    }
    
    public static class ElementMixerFactory
    {
        public static IElementMixer Mixer { get; } = new LocalElementMixer();
    }

    public class LocalElementMixer : IElementMixer
    {
        public ElementData MixElements(IEnumerable<ElementData> elementsData)
        {
            return new ElementData
            {
                color = new Color(0.5f, 0.1f, 0.4f)
            };
        }
    }

    public interface IElementMixer
    {
        ElementData MixElements(IEnumerable<ElementData> elementsData);
    }
}