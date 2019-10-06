using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class ModalWindow : MonoBehaviour
    {
        private Image _image;
        private Text _text;
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
            _text = GetComponentInChildren<Text>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show(Sprite sprite, string text)
        {
            _image.sprite = sprite;
            _text.text = text;
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}