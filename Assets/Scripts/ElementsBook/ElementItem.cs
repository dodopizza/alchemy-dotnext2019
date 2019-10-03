using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ElementsBook
{
    public class ElementItem : MonoBehaviour, IPointerClickHandler
    {
        public GameObject floatingElementPrefab;
        
        private GameObject _canvas;
        private Sprite _sprite;
        private Image _image;

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _image.sprite = _sprite;
        }

        public void SetUp(GameObject canvas, Sprite sprite)
        {
            _canvas = canvas;
            _sprite = sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameManager.Instance.CheckAndLockInput())
            {
                var floatingElement = Instantiate(floatingElementPrefab, _canvas.transform);
                floatingElement.transform.position = _image.transform.position;

                floatingElement.GetComponent<FloatingElement>()
                    .SetUp(GameManager.Instance.GetMixElement(), _sprite);
            }
        }
    }
}