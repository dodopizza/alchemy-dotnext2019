using System.Collections;
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

        private void Start()
        {
            _initialPosition = transform.position;
            _finalPosition = _mixElement.transform.position;
            _waitForSeconds = duration * 0.05f;
            _finalPosition.z = _initialPosition.z;
            
            GameManager.Instance.HandleUiCoroutine(GoToMixPosition());
        }

        private IEnumerator GoToMixPosition()
        {
            float t = 0;

            while (t <= 1)
            {
                transform.position = Vector3.Lerp(_initialPosition, _finalPosition, t);
                t += 0.05f;
                yield return new WaitForSeconds(_waitForSeconds);
            }
            
            _mixElement.ChangeElement(_sprite);
            Destroy(gameObject);
        }
    }
}