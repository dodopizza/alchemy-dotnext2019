using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

namespace MainScreen
{
    public class MixPoint : MonoBehaviour
    {
        private Image _elementImage;
        public float duration = 1f;

        private void Start()
        {
            _elementImage = transform.GetChild(0).GetComponent<Image>();
        }
        
        public async UniTask ChangeSprite(Sprite sprite, bool isFirstCreated)
        {
            _elementImage.sprite = sprite;
            _elementImage.color = Color.white;

            if (!isFirstCreated)
            {
                await EraseElementAnimation();
            }
        }

        public void Erase()
        {
            _elementImage.color = Color.clear;
        }
        
        private async UniTask EraseElementAnimation()
        {
            float t = 0;
            
            var waitForSeconds = duration * 0.05f;
            var ms = (int)(waitForSeconds * 1000);

            await UniTask.Delay(ms * 10);

            while (t <= 1)
            {
                _elementImage.color = Color.Lerp(Color.white, Color.clear, t);
                t += 0.05f;
                await UniTask.Delay(ms);
            }
            
            _elementImage.color = Color.clear;
        }
    }
}