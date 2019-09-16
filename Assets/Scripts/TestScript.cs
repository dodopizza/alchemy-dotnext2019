using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : LeanSelectableBehaviour
{
    /// <summary>Automatically read the DefaultColor from the SpriteRenderer?</summary>
    [Tooltip("Automatically read the DefaultColor from the SpriteRenderer?")]
    public bool AutoGetDefaultColor;

    /// <summary>The default color given to the SpriteRenderer.</summary>
    [Tooltip("The default color given to the SpriteRenderer.")]
    public Color DefaultColor = Color.white;

    /// <summary>The color given to the SpriteRenderer when selected.</summary>
    [Tooltip("The color given to the SpriteRenderer when selected.")]
    public Color SelectedColor = Color.green;

    protected virtual void Awake()
    {
        if (AutoGetDefaultColor == true)
        {
            var text = GetComponent<Text>();

            DefaultColor = text.color;
        }
    }

    protected override void OnSelect(LeanFinger finger)
    {
        Debug.Log("we're in!");
        ChangeColor(SelectedColor);
    }

    protected override void OnDeselect()
    {
        ChangeColor(DefaultColor);
    }

    private void ChangeColor(Color color)
    {
        var text = GetComponent<Text>();

        text.text = color.ToString();
    }
}
