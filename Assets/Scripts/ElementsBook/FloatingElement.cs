using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ElementsBook
{
    public class FloatingElement : MonoBehaviour
    {
        public GameObject mixElement;
        public float duration = 1f;
        
        public Image Image { get; private set; }

        private Vector3 initialPosition;
        private Vector3 finalPosition;
        private float waitForSeconds;

        private void Awake()
        {
            Image = transform.GetComponent<Image>();
        }

        private void Start()
        {
            initialPosition = transform.position;
            finalPosition = mixElement.transform.position;
            waitForSeconds = duration * 0.05f;
            finalPosition.z = initialPosition.z;
            
            StartCoroutine(GoToMixPosition());
        }

        private IEnumerator GoToMixPosition()
        {
            float t = 0;

            while (t <= 1)
            {
                transform.position = Vector3.Lerp(initialPosition, finalPosition, t);
                t += 0.05f;
                yield return new WaitForSeconds(waitForSeconds);
            }
            
            mixElement.GetComponent<Image>().sprite = Image.sprite;
            Destroy(gameObject);
        }
    }
}