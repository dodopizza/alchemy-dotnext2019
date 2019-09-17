using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class Element : LeanSelectableBehaviour
{
    private readonly Color _selectedColor = Color.green;
    
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
        ChangeColor(DefaultColor);
    }

    public Color DefaultColor { get; set; }

    private void ChangeColor(Color color)
    {
        var graphic = GetComponent<Graphic>();

        graphic.color = color;
    }

    public void SetElementData(ElementData data)
    {
        DefaultColor = data.color;
        ChangeColor(DefaultColor);
    }
}
