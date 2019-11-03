using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class InfoText : MonoBehaviour
    {
        private GameManager.ScoresAdd _instanceOnOnScoresAdd;
        private void Awake()
        {
            var textComponent = GetComponentInChildren<Text>();

            _instanceOnOnScoresAdd = (scores, elements) =>
            {
                textComponent.text = $"[Очки:{scores}]" 
                                     + Environment.NewLine 
                                     + Environment.NewLine 
                                     + $"[Открыто {elements}/128]";
            };
            GameManager.Instance.OnScoresAdd += _instanceOnOnScoresAdd;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnScoresAdd -= _instanceOnOnScoresAdd;
        }
    }
}