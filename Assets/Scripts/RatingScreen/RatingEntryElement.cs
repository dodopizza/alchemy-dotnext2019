using UnityEngine;
using UnityEngine.UI;

public class RatingEntryElement : MonoBehaviour
{
    public void Setup(Domain.Models.RatingEntry entry, bool isSelf)
    {
        var text = GetComponent<Text>();
        text.text = $"{entry.Position}.{entry.Nickname}:{entry.Score}";
        
        if (isSelf)
        {
            text.color = Color.red;
            text.fontSize = 30;
        }
    }
}
