using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class ModalWindow : MonoBehaviour
    {
        public float duration = 0.2f;
        public Image image;
        
        private Text _text;
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _text = GetComponentInChildren<Text>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public async Task Show(Sprite sprite, string text)
        {
            image.sprite = sprite;
            _text.text = text;
            await Show();
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        private async Task Show()
        {
            float t = 0;
            
            var waitForSeconds = duration * 0.05f;
            var ms = (int)(waitForSeconds * 1000);

            while (t <= 1)
            {
                _canvasGroup.alpha = t;
                t += 0.05f;
                await Task.Delay(ms);
            }
            _canvasGroup.alpha = 1f;
        }
    }
}