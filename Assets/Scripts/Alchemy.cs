using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using ElementProvider;
using UnityEngine;
using UnityEngine.UI;

public class Alchemy : MonoBehaviour
{
    public GameObject elementPrefabType;
    public GameObject elementsParent;
    
    // Start is called before the first frame update
    void Start()
    {
        var elementProvider = ElementProviderFactory.Provider;
        var elements = elementProvider.GetElements();

        foreach (var elementData in elements)
        {
            var element = Instantiate(elementPrefabType, elementsParent.transform);

            element.GetComponent<Element>().SetElementData(elementData);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}