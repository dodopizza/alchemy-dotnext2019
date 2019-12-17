using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Interfaces;
using Domain.Local;
using Domain.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx.Async;

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

        public void UnlockInput()
        {
            _inputLocked = false;
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
            SetDragThreshold();

            #if UNITY_IOS
            Destroy(GameObject.Find("ExitButton"));
            #endif
            
            var csvWorker = new CsvWorker();
            _recipeBook = new RecipeBook(csvWorker);
            _forge = new Forge(_recipeBook, csvWorker);

            _elementItemPrefab = (GameObject) Resources.Load("Prefabs/ElementItem", typeof(GameObject));
            _newElementWindowPrefab = (GameObject) Resources.Load("Prefabs/NewElementWindow", typeof(GameObject));
            _somethingWrongWindowPrefab = (GameObject) Resources.Load("Prefabs/SomethingWrongWindow", typeof(GameObject));
            _confirmExitWindowPrefab = (GameObject) Resources.Load("Prefabs/ConfirmExitWindow", typeof(GameObject));
            
            var loadInitialElements = await _recipeBook.LoadInitialElements();

            if (loadInitialElements.IsSuccess)
            {
                var initialElements = loadInitialElements.Data.ToList();
                InitializeElements(initialElements);
                InitializeScores(initialElements);
            }
            else
            {
                Instantiate(_somethingWrongWindowPrefab, UnderUpperLayerTransform);
            }
        }

        private static void SetDragThreshold()
        {
            var defaultValue = EventSystem.current.pixelDragThreshold;
            EventSystem.current.pixelDragThreshold = Mathf.Max(defaultValue, (int) (defaultValue * Screen.dpi / 160f));
        }


        private void Update()
        {
#if UNITY_IOS
#else
            if (Input.GetKeyDown(KeyCode.Escape) && !_inputLocked)
            {
                Exit();
            }
#endif

        }

        public void Exit()
        {
            Instantiate(_confirmExitWindowPrefab, UnderUpperLayerTransform);
        }
        
        public async UniTask PerformMix()
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
                    await UniTask.WhenAll(forgeSlotOne.Mix(), forgeSlotTwo.Mix());

                    if (mixResult.IsCrash)
                    {
                        //Easter egg
                        Debug.Log("Easter egg");
                        Application.Quit();
                    }

                    await mixPoint.ChangeSprite(mixResult.Element.Sprite, mixResult.IsNewlyCreated);

                    if (mixResult.IsNewlyCreated)
                    {
                        var element = mixResult.Element;
                        var newElementWindow = Instantiate(_newElementWindowPrefab, UnderUpperLayerTransform)
                            .GetComponent<NewElementWindow>();
                            
                        newElementWindow.Initialize(element.Sprite, element.Name, element.Score, element.Description);
                        await newElementWindow.Show();
                        mixPoint.Erase();
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

        public async UniTask HandleUiOperation(UniTask uiOperation)
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
            var newElement = Instantiate(_elementItemPrefab, elementsBook.transform);
                
            newElement.GetComponent<BookElementItem>().SetUp(element);
//            SortBook(newElement.transform);
        }

        private void SortBook(Transform newElementTransform)
        {
            var parentTransform = elementsBook.transform;
            var numberChildren = parentTransform.childCount;

            for (var i = 0; i < numberChildren; i++)
            {
                var compareOrdinal = string.CompareOrdinal(parentTransform.GetChild(i).name, newElementTransform.name);
                if (compareOrdinal <= 0) continue;
                newElementTransform.SetSiblingIndex(i);
                return;
            }
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