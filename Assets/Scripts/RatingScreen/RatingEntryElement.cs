using System;
using UnityEngine;
using UnityEngine.UI;

namespace RatingScreen
{
    public class RatingEntryElement : MonoBehaviour
    {
        public void Setup(Domain.Models.RatingEntry entry, bool isSelf)
        {
            var text = GetComponent<Text>();
            text.text = $"{entry.Position}.{entry.Nickname}:{entry.Score}";

            if (isSelf)
            {
                text.color = new Color32(255, 124, 36, Byte.MaxValue	);
                text.fontSize = 35;
            }
        }
    }
}
