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

        private bool _inputLocked;

        public bool CheckAndLockInput()
        {
            if (_inputLocked) 
                return false;
            
            _inputLocked = true;
            return true;
        }

        public Transform CanvasTransform => canvas.transform;
        
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
                if (Random.Range(0, 2) == 1)
                {
                    await Task.WhenAll(mixElementOne.Mix(), mixElementTwo.Mix());

                    var elements = LoadElements().ToArray();
                    Instantiate(elementItemPrefab, elementsBook.transform)
                        .GetComponent<ElementItem>()
                        .SetUp(elements[Random.Range(0, elements.Length)]);
                }
                else
                    await mixElementTwo.Erase();
                //await Task.WhenAll(mixElementOne.Erase(), mixElementTwo.Erase());
            }
        }

        public async Task HandleUiOperation(Task uiOperation)
        {
            _inputLocked = true;
            await uiOperation;
            _inputLocked = false;
        }

        private void InitializeElements()
        {
            var elementSprites = LoadElements();

            foreach (var sprite in elementSprites)
            {
                Instantiate(elementItemPrefab, elementsBook.transform)
                    .GetComponent<ElementItem>()
                    .SetUp(sprite);
            }
        }

        private IEnumerable<Sprite> LoadElements()
        {
            var loadedSprites = Resources.LoadAll("Sprites/Elements", typeof(Sprite));
            return loadedSprites.Select(s => (Sprite) s);
        }
    }
}