using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Alchemy : MonoBehaviour
{
    public GameObject prefabType;
    public Canvas canvas;
    
    public ElementData[] elements = new ElementData[]
    {
        new ElementData() {color = Color.red},
        new ElementData() {color = Color.blue},
    };

    // Start is called before the first frame update
    void Start()
    {
        var element = Instantiate(prefabType, canvas.transform);
        element.GetComponent<Image>().color = elements[1].color;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

[Serializable]
public class ElementData
{
    public Color color;
}