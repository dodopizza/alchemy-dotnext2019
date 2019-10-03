using System.Collections;
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

        public void ChangeElement(Sprite sprite)
        {
            _backImage.color = Color.clear;
            _elementImage.sprite = sprite;
            _elementImage.color = Color.white;

            IsEmpty = false;
            GameManager.Instance.PerformMix();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (GameManager.Instance.CheckAndLockInput())
                EraseElement();
        }

        public void EraseElement()
        {
            if (!IsEmpty)
            {
                GameManager.Instance.HandleUiCoroutine(ClearElementCoroutine());
                IsEmpty = true;
            }
        }
        
        private IEnumerator ClearElementCoroutine()
        {
            float t = 0;

            while (t <= 1)
            {
                _elementImage.color = Color.Lerp(Color.white, Color.clear, t);
                _backImage.color = Color.Lerp(Color.clear, Color.white, t);
                t += 0.05f;
                yield return new WaitForSeconds(_waitForSeconds);
            }
            
            _elementImage.color = Color.clear;
            _backImage.color = Color.white;
        }
    }
}