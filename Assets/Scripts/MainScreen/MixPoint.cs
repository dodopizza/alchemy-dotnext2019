using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;
using Utils;

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
            await AnimationRunner.Run((_) => { }, duration / 10);

            await AnimationRunner.Run(
                (t) =>
                {
                    _elementImage.color = Color.Lerp(Color.white, Color.clear, t);
                }, duration);

            _elementImage.color = Color.clear;
        }
    }
}