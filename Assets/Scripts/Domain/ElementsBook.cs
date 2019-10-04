using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Domain
{
    class ElementsBook : IElementsBook
    {
        private readonly List<Element> _elements = new List<Element>();
        private readonly List<Element> _openedElements = new List<Element>();

        public ElementsBook()
        {
            LoadElements();
        }
        
        public Element[] GetOpenedElements()
        {
            return _openedElements.ToArray();
        }

        public Element OpenAndGetElement(Guid elementId)
        {
            var element = _elements.First(); //e => e.Id == elementId);
            
            if (!ElementOpened(elementId))
            {
                _openedElements.Add(element);
            }

            return element;
        }

        public bool ElementOpened(Guid elementId)
        {
            return _openedElements.Any(x => x.Id == elementId);
        }

        private void LoadElements()
        {
            _elements.Clear();
            _elements.AddRange(LoadSprites().Select(s => new Element
            {
                Sprite = s
            }));
            
            _openedElements.Clear();
            _openedElements.AddRange(_elements.Skip(1));
        }
        
        private IEnumerable<Sprite> LoadSprites()
        {
            var loadedSprites = Resources.LoadAll("Sprites/Elements", typeof(Sprite));
            return loadedSprites.Select(s => (Sprite) s);
        }
    }
}