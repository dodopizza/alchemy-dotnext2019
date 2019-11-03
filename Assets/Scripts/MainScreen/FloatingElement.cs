using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Debug = UnityEngine.Debug;

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
            await AnimationRunner.Run(
                (t) =>
                {
                    transform.position = Vector3.Lerp(_initialPosition, _finalPosition, Mathf.SmoothStep(0f, 1f, t));
                }, duration);

            Destroy(gameObject);

            if (_actionAfter != null)
                await _actionAfter();
        }
    }
}