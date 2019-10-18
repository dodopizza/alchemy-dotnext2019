using System.Threading.Tasks;
using Domain;
using Domain.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class GameManager : MonoBehaviour
    {
        public ForgeSlot forgeSlotOne;
        public ForgeSlot forgeSlotTwo;
        public GameObject canvas;
        public GameObject elementItemPrefab;
        public GameObject elementsBook;
        public ModalWindow modalWindow;

        private IReceiptsBook _receiptsBook;
        private IForge _forge;
        
        public static GameManager Instance { get; private set; }

        private bool _inputLocked;

        public bool CheckAndLockInput()
        {
            if (_inputLocked) 
                return false;
            
            _inputLocked = true;
            return true;
        }

        public Transform CanvasTransform => canvas.transform;

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _receiptsBook = new ReceiptsBook();
            _forge = new Forge(_receiptsBook, new NetworkMixChecker());
            InitializeElements();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        public ForgeSlot GetMixElement()
        {
            return forgeSlotOne.IsEmpty ? forgeSlotOne : forgeSlotTwo;
        }

        public async Task PerformMix()
        {
            var resultTask = _forge.GetMixResult();
            if (resultTask == null)
                return;
            
            var operationResult = await resultTask;
            // todo: логика, когда что-то идёт не так

            if (operationResult.IsSuccess)
            {
                var mixResult = operationResult.Data;
                if (mixResult.IsSuccess)
                {
                    await Task.WhenAll(forgeSlotOne.Mix(), forgeSlotTwo.Mix());

                    if (mixResult.IsNewlyCreated)
                    {
                        await modalWindow.Show($"Вы собрали {mixResult.Element.Name}!", "Описание");
                        AddNewElement(mixResult.Element);
                    }
                }
                else
                {
                    await forgeSlotTwo.Erase();
                }
            }
            else
            {
                await modalWindow.Show(null, "Что-то пошло не так!");
                forgeSlotTwo.Erase();
            }
        }

        public async Task HandleUiOperation(Task uiOperation)
        {
            Debug.Log("lock");
            _inputLocked = true;
            await uiOperation;
            _inputLocked = false;
            Debug.Log("release");
        }

        private void AddNewElement(Element element)
        {
            Instantiate(elementItemPrefab, elementsBook.transform)
                .GetComponent<BookElementItem>()
                .SetUp(element);
        }
        
        private void InitializeElements()
        {
            var openedElements = _receiptsBook.GetOpenedElements();

            foreach (var element in openedElements)
            {
                Instantiate(elementItemPrefab, elementsBook.transform)
                    .GetComponent<BookElementItem>()
                    .SetUp(element);
            }
        }

        public void AddElementToForge(Element element)
        {
            _forge.AddElement(element);
        }

        public void ClearForge()
        {
            _forge.Clear();
        }
    }
}