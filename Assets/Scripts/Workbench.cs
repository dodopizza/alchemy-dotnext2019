using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    public int elementsMaximum = 2;
    public List<Element> currentElements = new List<Element>();
    public GameObject elementPrefabType;
    
    public void AddElement(ElementData elementData)
    {
        if (currentElements.Count >= elementsMaximum)
        {
            return;
        }
        
        var elementObject = Instantiate(elementPrefabType, transform);
        var element = elementObject.GetComponent<Element>();
        element.Init(elementData);
        currentElements.Add(element);
    }
}
