using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class Element : LeanSelectableBehaviour
{
    private Color _selectedColor = Color.green;
    private Color _defaultColor;

    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        
    }

    protected override void OnSelect(LeanFinger finger)
    {
        ChangeColor(_selectedColor);
    }

    protected override void OnDeselect()
    {
        ChangeColor(_defaultColor);
    }

    private void ChangeColor(Color color)
    {
        var graphic = GetComponent<Graphic>();
        graphic.color = color;
    }

    public void SetElementData(ElementData data)
    {
        _defaultColor = data.color;
        ChangeColor(_defaultColor);
    }
}
