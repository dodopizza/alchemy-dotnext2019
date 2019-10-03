using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ElementsBook
{
    public class FloatingElement : MonoBehaviour
    {
        public float duration = 1f;

        private MixElement _mixElement;
        private Sprite _sprite;
        private Vector3 _initialPosition;
        private Vector3 _finalPosition;
        private float _waitForSeconds;

        public void SetUp(MixElement mixElement, Sprite sprite)
        {
            _mixElement = mixElement;
            _sprite = sprite;
            transform.GetComponent<Image>().sprite = sprite;
        }

        private async void Start()
        {
            _initialPosition = transform.position;
            _finalPosition = _mixElement.transform.position;
            _waitForSeconds = duration * 0.05f;
            _finalPosition.z = _initialPosition.z;

            await GameManager.Instance.HandleUIOperation(GoToMixPosition());
        }

        private async Task GoToMixPosition()
        {
            float t = 0;
            var ms = (int)(_waitForSeconds * 1000);

            while (t <= 1)
            {
                transform.position = Vector3.Lerp(_initialPosition, _finalPosition, t);
                t += 0.05f;
                await Task.Delay(ms);
            }
            Destroy(gameObject);
            await _mixElement.ChangeElement(_sprite);
        }
    }
}