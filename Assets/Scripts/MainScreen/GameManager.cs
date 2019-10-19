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

        private IRecipeBook _recipeBook;
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

        private async void Start()
        {
            _recipeBook = new RecipeBook();
            _forge = new Forge(_recipeBook, new NetworkMixChecker());

            await _recipeBook.LoadInitialElements();
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
                        var element = mixResult.Element;
                        await modalWindow.Show($"Вы собрали {element.Name}!", element.Description);
                        AddElementScores(element.Score);
                        AddNewElement(element);
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

        public async void EraseData()
        {
//            await (_recipeBook as RecipeBook).TryLoad();
            Persistence.EraseData();
            PlayerPrefs.SetInt(Constants.UserScoresKey, 0);
            PlayerPrefs.SetInt(Constants.OpenedElementsKey, 4);
        }
        
        private void AddNewElement(Element element)
        {
            Instantiate(elementItemPrefab, elementsBook.transform)
                .GetComponent<BookElementItem>()
                .SetUp(element);
        }
        
        private void InitializeElements()
        {
            var openedElements = _recipeBook.GetOpenedElements();

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