using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class ModalWindow : MonoBehaviour
    {
        public float duration = 0.2f;
        
        public Text elementName;
        public Text elementDescription;
        public MixPoint mixPoint;
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public async Task Show(string elementName, string elementDescription)
        {
            this.elementName.text = elementName;
            this.elementDescription.text = elementDescription;
            await Show();
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            mixPoint.Erase();
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