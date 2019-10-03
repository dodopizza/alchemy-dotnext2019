using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ElementsBook
{
    public class ElementItem : MonoBehaviour, IPointerClickHandler
    {
        public GameObject floatingElementPrefab;
        public GameManager GameManager { get; set; }
        public GameObject Canvas { get; set; }
        public Sprite Sprite { get; set; }

        private Image image;

        private void Start()
        {
            image = GetComponentInChildren<Image>();
            image.sprite = Sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var floatingElement = Instantiate(floatingElementPrefab, Canvas.transform);
            floatingElement.transform.position = image.transform.position;
            
            var floatingElementScript = floatingElement.GetComponent<FloatingElement>();
            floatingElementScript.Image.sprite = image.sprite;
            floatingElementScript.mixElement = GameManager.GetMixElement();
        }
    }
}