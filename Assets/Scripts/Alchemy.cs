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

        foreach (var elementData in elements)
        {
            elementsBook.AddElement(elementData, ElementSelected);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ElementSelected(ElementData element)
    {
        workbench.AddElement(element);
    }
}