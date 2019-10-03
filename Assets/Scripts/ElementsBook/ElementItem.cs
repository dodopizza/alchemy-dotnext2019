using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ElementsBook
{
    public class ElementItem : MonoBehaviour, IPointerClickHandler
    {
        public GameObject floatingElementPrefab;

        private Sprite _sprite;
        private Image _image;

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _image.sprite = _sprite;
        }

        public void SetUp(Sprite sprite)
        {
            _sprite = sprite;
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (!GameManager.Instance.CheckAndLockInput()) 
                return;
            
            var mixElement = GameManager.Instance.GetMixElement();

            await Instantiate(floatingElementPrefab, GameManager.Instance.CanvasTransform)
                .GetComponent<FloatingElement>()
                .Run(
                    _image.transform.position,
                    mixElement.transform.position,
                    _sprite,
                    () => mixElement.ChangeElement(_sprite));
        }
    }
}