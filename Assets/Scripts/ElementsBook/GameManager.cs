using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ElementsBook
{
    // todo: singleton
    public class GameManager : MonoBehaviour
    {
        public MixElement mixElementOne;
        public MixElement mixElementTwo;
        public GameObject canvas;
        public GameObject elementItemPrefab;
        public GameObject elementsBook;

        public static GameManager Instance => instance;

        public bool CheckAndLockInput()
        {
            if (inputLocked) 
                return false;
            
            inputLocked = true;
            return true;
        }
        
        private bool inputLocked = false;

        private static GameManager instance;

        private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            InitializeElements();
        }

        public MixElement GetMixElement()
        {
            return mixElementOne.IsEmpty ? mixElementOne : mixElementTwo;
        }

        public async Task PerformMix()
        {
            if (!mixElementOne.IsEmpty && !mixElementTwo.IsEmpty)
            {
                await Task.WhenAll(mixElementOne.EraseElement(), mixElementTwo.EraseElement());
            }
        }

        public async Task HandleUIOperation(Task uiOperation)
        {
            inputLocked = true;
            await uiOperation;
            inputLocked = false;
        }

        private void InitializeElements()
        {
            var elementSprites = LoadElements();

            foreach (var sprite in elementSprites)
            {
                var element = Instantiate(elementItemPrefab, elementsBook.transform);
                element.GetComponent<ElementItem>()
                    .SetUp(canvas, sprite);
            }
        }

        private IEnumerable<Sprite> LoadElements()
        {
            var loadedSprites = Resources.LoadAll("Sprites/Elements", typeof(Sprite));
            return loadedSprites.Select(s => (Sprite) s);
        }
    }
}