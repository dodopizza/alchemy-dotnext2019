using System.Threading.Tasks;
using Domain;
using Domain.Models;
using UnityEngine;

namespace MainScreen
{
    public class GameManager : MonoBehaviour
    {
        public ForgeSlot forgeSlotOne;
        public ForgeSlot forgeSlotTwo;
        public MixPoint mixPoint;
        public GameObject canvas;
        public GameObject elementItemPrefab;
        public GameObject elementsBook;
        public ModalWindow modalWindow;

        private IReceiptsBook _receiptsBook;
        private IForge _forge;
        
        public static GameManager Instance { get; private set; }

        private bool _inputLocked;
        private int _scores;
        private int _openedElements;

        public bool CheckAndLockInput()
        {
            if (_inputLocked) 
                return false;
            
            _inputLocked = true;
            return true;
        }

        public Transform CanvasTransform => canvas.transform;
        
        public ForgeSlot EmptyForgeSlot => forgeSlotOne.IsEmpty ? forgeSlotOne : forgeSlotTwo;

        public delegate void ScoresAdd(int scores, int openedElements);

        public event ScoresAdd OnScoresAdd;

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
            InitializeScores();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Exit();
        }

        public async Task PerformMix()
        {
            var resultTask = _forge.GetMixResult();
            if (resultTask == null)
                return;
            
            var operationResult = await resultTask;

            if (operationResult.IsSuccess)
            {
                var mixResult = operationResult.Data;
                if (mixResult.IsSuccess)
                {
                    await Task.WhenAll(forgeSlotOne.Mix(), forgeSlotTwo.Mix());
                    await mixPoint.ChangeSprite(mixResult.Element.Sprite, mixResult.IsNewlyCreated);

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
            _inputLocked = true;
            await uiOperation;
            _inputLocked = false;
        }

        public void AddElementToForge(Element element)
        {
            _forge.AddElement(element);
        }

        public void ClearForge()
        {
            _forge.Clear();
        }

        public void Exit()
        {
            Application.Quit();
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
        
        private void InitializeScores()
        {
            if (PlayerPrefs.HasKey(Constants.UserScoresKey))
            {
                _scores = PlayerPrefs.GetInt(Constants.UserScoresKey);
            }

            if (PlayerPrefs.HasKey(Constants.OpenedElementsKey))
            {
                _openedElements = PlayerPrefs.GetInt(Constants.OpenedElementsKey);
            }
            else
            {
                _openedElements = 4;
            }
            
            SaveAndUpdateScores();
        }

        private void AddElementScores(int scores)
        {
            _scores += scores;
            _openedElements++;
            
            SaveAndUpdateScores();
        }

        private void SaveAndUpdateScores()
        {
            PlayerPrefs.SetInt(Constants.UserScoresKey, _scores);
            PlayerPrefs.SetInt(Constants.OpenedElementsKey, _openedElements);
            PlayerPrefs.Save();
            OnScoresAdd?.Invoke(_scores, _openedElements);
        }
    }
}