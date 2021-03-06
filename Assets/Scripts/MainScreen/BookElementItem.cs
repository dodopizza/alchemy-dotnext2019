using System.Diagnostics;
using Domain.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx.Async;
using Debug = UnityEngine.Debug;

namespace MainScreen
{
    public class BookElementItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public GameObject floatingElementPrefab;

        private Image _image;
        private Element _element;
        private readonly GameManager _gameManager = GameManager.Instance;
        private bool _held;
        private GameObject _newElementWindowPrefab;

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _image.sprite = _element.Sprite;
            _newElementWindowPrefab = (GameObject) Resources.Load("Prefabs/NewElementWindow", typeof(GameObject));

            GetComponentInChildren<Text>().text = _element.Name;
        }

        public void SetUp(Element element)
        {
            _element = element;
            transform.name = element.Name;
        }

        private UniTask SendElementToForge(ForgeSlot mixElement)
        {
            return Instantiate(floatingElementPrefab, _gameManager.UnderUpperLayerTransform)
                .GetComponent<FloatingElement>()
                .Run(
                    _image.transform.position,
                    mixElement.transform.position,
                    _element.Sprite,
                    () => mixElement.ChangeSprite(_element.Sprite));
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            if (!_gameManager.CheckAndLockInput() || _held) 
                return;

            _gameManager.AddElementToForge(_element);
            
            var mixElement = _gameManager.EmptyForgeSlot;
            await _gameManager.HandleUiOperation(SendElementToForge(mixElement));
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _held = false;
            Invoke(nameof(OnLongPress), 0.5f);
        }
 
        public void OnPointerUp(PointerEventData eventData)
        {
            CancelInvoke(nameof(OnLongPress));
        }
 
        public void OnPointerExit(PointerEventData eventData)
        {
            CancelInvoke(nameof(OnLongPress));
        }
 
        private async UniTask OnLongPress()
        {
            _held = true;
            var newElementWindow = Instantiate(_newElementWindowPrefab, _gameManager.UnderUpperLayerTransform)
                .GetComponent<NewElementWindow>();
                                        
            newElementWindow.Initialize(_element.Sprite, _element.Name, _element.Score, _element.Description);
            await _gameManager.HandleUiOperation(newElementWindow.Show());
        }
    }
}