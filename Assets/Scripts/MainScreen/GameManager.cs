using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Domain.Models;
using UnityEngine;

namespace MainScreen
{
    public class GameManager : MonoBehaviour
    {
        public ForgeSlot forgeSlotOne;
        public ForgeSlot forgeSlotTwo;
        public MixPoint mixPoint;
        public GameObject underUpperLayer;
        public GameObject elementsBook;

        private GameObject _elementItemPrefab;
        private GameObject _newElementWindowPrefab;
        private GameObject _confirmExitWindowPrefab;
        private GameObject _somethingWrongWindowPrefab;

        private IRecipeBook _recipeBook;
        private IForge _forge;
        
        public static GameManager Instance { get; private set; }

        private bool _inputLocked;
        private int _score;
        private int _openedElements;

        public bool CheckAndLockInput()
        {
            if (_inputLocked) 
                return false;
            
            _inputLocked = true;
            return true;
        }

        public Transform UnderUpperLayerTransform => underUpperLayer.transform;
        
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

            _elementItemPrefab = (GameObject) Resources.Load("Prefabs/ElementItem", typeof(GameObject));
            _newElementWindowPrefab = (GameObject) Resources.Load("Prefabs/NewElementWindow", typeof(GameObject));
            _somethingWrongWindowPrefab = (GameObject) Resources.Load("Prefabs/SomethingWrongWindow", typeof(GameObject));
            _confirmExitWindowPrefab = (GameObject) Resources.Load("Prefabs/ConfirmExitWindow", typeof(GameObject));
            
            var initialElements = await _recipeBook.LoadInitialElements();
            InitializeElements(initialElements);
            InitializeScores(initialElements);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Exit();
            }
        }

        public void Exit()
        {
            Instantiate(_confirmExitWindowPrefab, UnderUpperLayerTransform);
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
                        var newElementWindow = Instantiate(_newElementWindowPrefab, UnderUpperLayerTransform)
                            .GetComponent<NewElementWindow>();
                            
                        newElementWindow.Initialize(
                            mixPoint, 
                            $"Вы собрали {element.Name}!", 
                            element.Description);
                        await newElementWindow.Show();
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
                Instantiate(_somethingWrongWindowPrefab, UnderUpperLayerTransform);
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

        public void EraseData()
        {
            Persistence.EraseData();
            PlayerPrefs.DeleteAll();
        }
        
        private void AddNewElement(Element element)
        {
            Instantiate(_elementItemPrefab, elementsBook.transform)
                .GetComponent<BookElementItem>()
                .SetUp(element);
        }
        
        private void InitializeElements(IEnumerable<Element> elements)
        {
            foreach (var element in elements)
            {
                AddNewElement(element);
            }
        }
        
        private void InitializeScores(IReadOnlyCollection<Element> elements)
        {
            _score = elements.Sum(e => e.Score);
            _openedElements = elements.Count;

            SaveAndUpdateScores();
        }

        private void AddElementScores(int scores)
        {
            _score += scores;
            _openedElements++;
            
            SaveAndUpdateScores();
        }

        private void SaveAndUpdateScores()
        {
            PlayerPrefs.SetInt(Constants.UserScoresKey, _score);
            PlayerPrefs.SetInt(Constants.OpenedElementsKey, _openedElements);
            PlayerPrefs.Save();
            OnScoresAdd?.Invoke(_score, _openedElements);
        }
    }
}