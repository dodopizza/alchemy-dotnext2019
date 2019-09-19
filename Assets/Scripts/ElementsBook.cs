using System;
using UnityEngine;

public class ElementsBook : MonoBehaviour
{
    public GameObject elementPrefabType;

    public void AddElement(ElementData elementData, Action<ElementData> onElementSelect)
    {
        var elementObject = Instantiate(elementPrefabType, transform);

        elementObject.GetComponent<Element>().Init(elementData, onElementSelect);
    }
}