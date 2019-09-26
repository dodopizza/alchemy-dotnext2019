using System;
using System.Collections.Generic;
using System.Linq;
using ElementsBook;
using UnityEngine;

public class Alchemy : MonoBehaviour
{
    public ElementsBook.ElementsBook elementsBook;
    public Workbench.Workbench workbench;
    private Dictionary<string, Sprite> sprites;

    // Start is called before the first frame update
    async void Start()
    {
        var elementProvider = ElementProviderFactory.Provider;
        var elements = await elementProvider.GetElements();
        sprites = Resources.LoadAll<Sprite>("japanese1kana").ToDictionary(sprite => sprite.name);
        
        workbench.Init(MixSuccess);
        elementsBook.Init(ElementSelected);

        foreach (var elementData in elements)
        {
            elementsBook.AddElement(elementData);
        }
    }

    private void ElementSelected(ElementData element)
    {
        workbench.AddElement(element);
    }
    
    private bool MixSuccess(ElementData elementData)
    {
        return elementsBook.TryAddElement(elementData);
    }

    public Sprite GetSprite(string dataSpriteName)
    {
        if (sprites.TryGetValue(dataSpriteName, out var sprite))
        {
            return sprite;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}