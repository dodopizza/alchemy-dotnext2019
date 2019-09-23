using ElementsBook;
using UnityEngine;

public class Alchemy : MonoBehaviour
{
    public ElementsBook.ElementsBook elementsBook;
    public Workbench.Workbench workbench;
    
    // Start is called before the first frame update
    async void Start()
    {
        var elementProvider = ElementProviderFactory.Provider;
        var elements = await elementProvider.GetElements();
        
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