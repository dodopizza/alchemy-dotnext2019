using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ElementsBook
{
    public class MixElement : MonoBehaviour, IPointerClickHandler
    {
        public float duration = 1f;

        private float _waitForSeconds;
        private Image _backImage;
        private Image _elementImage;

        public bool IsEmpty { get; private set; } = true;

        private void Start()
        {
            _backImage = transform.GetComponent<Image>();
            _elementImage = transform.GetChild(0).GetComponent<Image>();
            _waitForSeconds = duration * 0.05f;
        }

        public async Task ChangeElement(Sprite sprite)
        {
            _backImage.color = Color.clear;
            _elementImage.sprite = sprite;
            _elementImage.color = Color.white;

            IsEmpty = false;
            await GameManager.Instance.PerformMix();
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (GameManager.Instance.CheckAndLockInput())
                await EraseElement();
        }

        public async Task EraseElement()
        {
            if (!IsEmpty)
            {
                await GameManager.Instance.HandleUIOperation(ClearElement());
                IsEmpty = true;
            }
        }
        
        private async Task ClearElement()
        {
            float t = 0;
            var ms = (int)(_waitForSeconds * 1000);

            while (t <= 1)
            {
                _elementImage.color = Color.Lerp(Color.white, Color.clear, t);
                _backImage.color = Color.Lerp(Color.clear, Color.white, t);
                t += 0.05f;
                await Task.Delay(ms);
            }
            
            _elementImage.color = Color.clear;
            _backImage.color = Color.white;
        }
    }
}