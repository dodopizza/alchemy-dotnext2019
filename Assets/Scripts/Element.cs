using System;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class Element : LeanSelectableBehaviour
{
    private Color _selectedColor = Color.green;
    private Color _defaultColor;
    private Alchemy _gameMaster;

    public ElementData elementData;

    private Action<ElementData> _onElementSelect;
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
        if (_onElementSelect == null)
        {
            return;
        }
        
        ChangeColor(_selectedColor);
        _onElementSelect(elementData);
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

    public void Init(ElementData data, Action<ElementData> onElementSelect)
    {
        _onElementSelect = onElementSelect;
        Init(data);        
    }
    
    public void Init(ElementData data)
    {
        elementData = data;
        
        _defaultColor = data.color;
        ChangeColor(_defaultColor);
    }
}
