using UnityEngine;

public class Workbench : MonoBehaviour
{
    public GameObject elementPrefabType;
    
    public void AddElement(ElementData elementData)
    {
        var elementObject = Instantiate(elementPrefabType, transform);

        elementObject.GetComponent<Element>().Init(elementData);
    }
}
