using System;
using UnityEngine;

public class ElementsBook : MonoBehaviour
{
    public GameObject elementPrefabType;
    private Action<ElementData> _onElementSelect;

    public void Init(Action<ElementData> onElementSelect)
    {
        _onElementSelect = onElementSelect;
    }
    
    public void AddElement(ElementData elementData)
    {
        var elementObject = Instantiate(elementPrefabType, transform);

        elementObject.GetComponent<Element>().Init(elementData, _onElementSelect);
    }

    public bool TryAddElement(ElementData elementData)
    {
        AddElement(elementData);
        return true;
    }
}