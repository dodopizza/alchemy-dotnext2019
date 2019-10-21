using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingEntryElement : MonoBehaviour
{
    public Text text;
    public Graphic background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup(Domain.Models.RatingEntry entry, bool isOwn)
    {
        text.text = $"{entry.Position}. {entry.Nickname}: {entry.Score}";
        if (isOwn)
        {
            if (ColorUtility.TryParseHtmlString("#32FF32", out var green))
            {
                background.color = green;
            }
            text.color = Color.black;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
