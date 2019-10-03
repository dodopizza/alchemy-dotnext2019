using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElementsBook
{
    // todo: singleton
    public class GameManager : MonoBehaviour
    {
        public GameObject mixElementOne;
        public GameObject mixElementTwo;
        public GameObject canvas;
        public GameObject elementItemPrefab;
        public GameObject elementsBook;
        
        private int i = 0;

        private void Start()
        {
            InitializeElements();
        }

        public GameObject GetMixElement()
        {
            return i++ % 2 == 0 ? mixElementOne : mixElementTwo;
        }

        private void InitializeElements()
        {
            var elementSprites = LoadElements();

            foreach (var sprite in elementSprites)
            {
                var element = Instantiate(elementItemPrefab, elementsBook.transform);
                var elementScript = element.GetComponent<ElementItem>();
                elementScript.Canvas = canvas;
                elementScript.GameManager = this;
                elementScript.Sprite = sprite;
            }
        }
        
        private IEnumerable<Sprite> LoadElements()
        {
            var loadedSprites = Resources.LoadAll("Sprites", typeof(Sprite));
            return loadedSprites.Select(s => (Sprite) s);
        }
    }
}