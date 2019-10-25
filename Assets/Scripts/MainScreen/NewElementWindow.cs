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
        public Image elementImage;
        private CanvasGroup _canvasGroup;

        public void Initialize(Sprite elementSprite, string elementName, string elementDescription)
        {
            elementImage.sprite = elementSprite;
            _canvasGroup = GetComponent<CanvasGroup>();
            this.elementName.text = elementName;
            this.elementDescription.text = elementDescription;
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
    }
}