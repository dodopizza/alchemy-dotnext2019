using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class NewElementWindow : MonoBehaviour
    {
        public float duration = 0.2f;
        
        public Text elementName;
        public Text elementDescription;
        public Text elementScore;
        public Image elementImage;
        private CanvasGroup _canvasGroup;

        public void Initialize(Sprite elementSprite, string name, int score, string description)
        {
            elementImage.sprite = elementSprite;
            _canvasGroup = GetComponent<CanvasGroup>();
            elementName.text = name;
            elementScore.text = $"{score} {ScoreEndings(score)}";
            elementDescription.text = description;
        }

        public async Task Show()
        {
            float t = 0;

            var ms = (int)(duration * 50);

            while (t <= 1)
            {
                _canvasGroup.alpha = t;
                t += 0.05f;
                await Task.Delay(ms);
            }
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
        
        public void Hide()
        {
            Destroy(gameObject);
        }

        private string ScoreEndings(int score)
        {
            var lastNumber = score % 10;

            switch (lastNumber)
            {
                case 1:
                    return "очко";
                case 2:
                case 3:
                case 4:
                    return "очка";
                default:
                    return "очков";
            }
        }
    }
}