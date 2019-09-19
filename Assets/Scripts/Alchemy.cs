using System.Collections;
using System.Collections.Generic;
using ElementProvider;
using UnityEngine;

public class Alchemy : MonoBehaviour
{
    public ElementsBook elementsBook;
    public Workbench workbench;
    
    // Start is called before the first frame update
    void Start()
    {
        var elementProvider = ElementProviderFactory.Provider;
        var elements = elementProvider.GetElements();
        
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
}