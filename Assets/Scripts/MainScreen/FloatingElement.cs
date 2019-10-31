using System;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class FloatingElement : MonoBehaviour
    {
        public float duration = 1f;

        private Vector3 _initialPosition;
        private Vector3 _finalPosition;
        private Func<UniTask> _actionAfter;

        public async UniTask Run(
            Vector3 initialPosition,
            Vector3 finalPosition,
            Sprite sprite,
            Func<UniTask> actionAfter)
        {
            _initialPosition = initialPosition;
            _finalPosition = finalPosition;
            _finalPosition.z = _initialPosition.z;

            _actionAfter = actionAfter;

            transform.position = _initialPosition;
            transform.GetComponent<Image>().sprite = sprite;

            await GoToMixPosition();
        }

        private async UniTask GoToMixPosition()
        {
            float t = 0;
            var ms = (int)(duration * 50);

            while (t <= 1)
            {
                transform.position = Vector3.Lerp(_initialPosition, _finalPosition, Mathf.SmoothStep(0f, 1f, t));
                t += 0.05f;
                await Task.Delay(ms);
            }
            Destroy(gameObject);

            if (_actionAfter != null)
                await _actionAfter();
        }
    }
}