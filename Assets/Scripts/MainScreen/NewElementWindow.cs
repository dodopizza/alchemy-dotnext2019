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
        private CanvasGroup _canvasGroup;
        private MixPoint _mixPoint;

        public void Initialize(MixPoint mixPoint, string elementName, string elementDescription)
        {
            _mixPoint = mixPoint;
            _canvasGroup = GetComponent<CanvasGroup>();
            this.elementName.text = elementName;
            this.elementDescription.text = elementDescription;
        }

        public async Task Show()
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
            _canvasGroup.blocksRaycasts = true;
        }
        
        public void Hide()
        {
            _mixPoint.Erase();
            Destroy(gameObject);
        }
    }
}