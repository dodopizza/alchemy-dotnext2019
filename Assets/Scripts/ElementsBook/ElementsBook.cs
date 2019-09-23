using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElementsBook
{
    public class ElementsBook : MonoBehaviour
    {
        public GameObject elementPrefabType;
        private Action<ElementData> _onElementSelect;
        private readonly List<Element> _knownElements = new List<Element>();

        public void Init(Action<ElementData> onElementSelect)
        {
            _onElementSelect = onElementSelect;
        }
    
        public void AddElement(ElementData elementData)
        {
            var elementObject = Instantiate(elementPrefabType, transform);

            var element = elementObject.GetComponent<Element>();
            element.Init(elementData, _onElementSelect);
            _knownElements.Add(element);
        }

        public bool TryAddElement(ElementData elementData)
        {
            if (_knownElements.Any(element => element.elementData.Color == elementData.Color))
            {
                Debug.Log("Element already exists!");
                return false;
            }

            AddElement(elementData);
            return true;
        }
    }
}
