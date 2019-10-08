using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class FloatingElement : MonoBehaviour
    {
        public float duration = 1f;

        private Vector3 _initialPosition;
        private Vector3 _finalPosition;
        private Func<Task> _actionAfter;

        public async Task Run(
            Vector3 initialPosition,
            Vector3 finalPosition,
            Sprite sprite,
            Func<Task> actionAfter)
        {
            _initialPosition = initialPosition;
            _finalPosition = finalPosition;
            _finalPosition.z = _initialPosition.z;

            _actionAfter = actionAfter;

            transform.position = _initialPosition;
            transform.GetComponent<Image>().sprite = sprite;

            await GoToMixPosition();
        }

        private async Task GoToMixPosition()
        {
            float t = 0;
            
            var waitForSeconds = duration * 0.05f;
            var ms = (int)(waitForSeconds * 1000);

            while (t <= 1)
            {
                transform.position = Vector3.Lerp(_initialPosition, _finalPosition, t);
                t += 0.05f;
                await Task.Delay(ms);
            }
            Destroy(gameObject);

            if (_actionAfter != null)
                await _actionAfter();
        }
    }
}