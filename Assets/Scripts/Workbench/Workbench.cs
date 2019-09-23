using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Workbench
{
    public class Workbench : MonoBehaviour
    {
        public int elementsMaximum = 2;
        public List<ElementData> currentElements = new List<ElementData>();
        public GameObject elementPrefabType;
        private Func<ElementData, bool> _onMixSuccess;
    
        public void Init(Func<ElementData, bool> onMixSuccess)
        {
            _onMixSuccess = onMixSuccess;
        }
   
        public void AddElement(ElementData elementData)
        {
            if (currentElements.Count >= elementsMaximum)
            {
                return;
            }
        
            var elementObject = Instantiate(elementPrefabType, transform);
            var element = elementObject.GetComponent<Element>();
            element.Init(elementData);
            currentElements.Add(element.elementData);
        }

        public void MixElements()
        {
            var elementsMixer = ElementMixerFactory.Mixer;
            var elementData = elementsMixer.MixElements(currentElements);

            if (elementData == null)
            {
                Debug.Log("Try Again");
            }
            else
            {
                if (_onMixSuccess(elementData))
                {
                    Debug.Log("Element added");
                }
                else
                {
                    Debug.Log("Element already exists");
                }
            }
        
            currentElements.Clear();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
