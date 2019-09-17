using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Alchemy : MonoBehaviour
{
    public GameObject prefabType;
    public GameObject elementContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        var elementProvider = ElementProviderFactory.Provider;
        var elements = elementProvider.GetElements();

        foreach (var elementData in elements)
        {
            var element = Instantiate(prefabType, elementContainer.transform);
            element.GetComponent<Image>().color = elementData.color;    
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}