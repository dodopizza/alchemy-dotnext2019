using System;
using UnityEngine;

[Serializable]
public class ElementData
{
    public float r;
    public float g;
    public float b;
    
    public Color Color => new Color(r, g, b);
}