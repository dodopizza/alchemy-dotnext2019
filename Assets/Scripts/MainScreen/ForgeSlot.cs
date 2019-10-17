using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainScreen
{
    public class ForgeSlot : MonoBehaviour, IPointerClickHandler
    {
        public float duration = 1f;
        public GameObject floatingElementPrefab;
        public GameObject mixPoint;

        private Image _backImage;
        private Image _elementImage;

        public bool IsEmpty { get; private set; } = true;

        private void Start()
        {
            _backImage = transform.GetComponent<Image>();
            _elementImage = transform.GetChild(0).GetComponent<Image>();
        }

        public async Task ChangeSprite(Sprite sprite)
        {
//            _backImage.color = Color.clear;
            _elementImage.sprite = sprite;
            _elementImage.color = Color.white;

            IsEmpty = false;
            await GameManager.Instance.PerformMix();
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (GameManager.Instance.CheckAndLockInput())
            {
                GameManager.Instance.ClearForge();
                await GameManager.Instance.HandleUiOperation(Erase());
            }
        }
        
        public async Task Erase()
        {
            if (!IsEmpty)
            {
                await EraseElementAnimation();
                IsEmpty = true;
            }
        }

        public async Task Mix()
        {
            await MixAnimation();
            IsEmpty = true;
        }
        
        private async Task EraseElementAnimation()
        {
            float t = 0;
            
            var waitForSeconds = duration * 0.05f;
            var ms = (int)(waitForSeconds * 1000);

            while (t <= 1)
            {
                _elementImage.color = Color.Lerp(Color.white, Color.clear, t);
//                _backImage.color = Color.Lerp(Color.clear, Color.white, t);
                t += 0.05f;
                await Task.Delay(ms);
            }
            
            _elementImage.color = Color.clear;
//            _backImage.color = Color.white;
        }

        private async Task MixAnimation()
        {
            _elementImage.color = Color.clear;
//            _backImage.color = Color.white;

            await Instantiate(floatingElementPrefab, GameManager.Instance.CanvasTransform)
                .GetComponent<FloatingElement>()
                .Run(
                    _elementImage.transform.position,
                    mixPoint.transform.position, 
                    _elementImage.sprite, 
                    null);
        }
    }
}