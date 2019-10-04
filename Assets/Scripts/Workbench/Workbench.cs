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
   
//        public void AddElement(ElementData elementData)
//        {
//            if (currentElements.Count >= elementsMaximum)
//            {
//                return;
//            }
//        
//            var elementObject = Instantiate(elementPrefabType, transform);
//            var element = elementObject.GetComponent<Element>();
//            element.Init(elementData);
//            currentElements.Add(element.elementData);
//        }

        public async void MixElements()
        {
            var elementsMixer = ElementMixerFactory.Mixer;
            var mixResult = await elementsMixer.MixElements(currentElements);

            if (!mixResult.success)
            {
                Debug.Log("Try Again");
            }
            else
            {
                if (_onMixSuccess(mixResult.result))
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
