using System;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class Element : LeanSelectableBehaviour
{
    private Alchemy _alchemy;

    public ElementData elementData;

    private Action<ElementData> _onElementSelect;

    protected override void OnSelect(LeanFinger finger)
    {
        _onElementSelect?.Invoke(elementData);
    }

    protected override void OnDeselect()
    {
        // use on deselect
    }

    private void ChangeSprite(Sprite sprite)
    {
        var graphic = GetComponent<Image>();
        graphic.sprite = sprite;
    }

    public void Init(ElementData data, Action<ElementData> onElementSelect)
    {
        _onElementSelect = onElementSelect;
        Init(data);
    }

    public void Init(ElementData data)
    {
        _alchemy = FindObjectOfType<Alchemy>();
        elementData = data;
        ChangeSprite(_alchemy.GetSprite(data.spriteName));
    }
}