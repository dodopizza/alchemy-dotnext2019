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

        public bool TryOpenElement(Guid elementId, out Element element)
        {
            // todo: переписать
            element = _elements.First(); //e => e.Id == elementId);

            var id = element.Id;
            
            if (_openedElements.All(x => x.Id != id))
            {
                _openedElements.Add(element);
                return true;
            }

            return false;
        }

        private void LoadElements()
        {
            _elements.Clear();
            _elements.AddRange(LoadSprites().Select(s => new Element
            {
                Id = Guid.NewGuid(),
                Sprite = s.sprite,
                Name = s.name
            }));
            
            _openedElements.Clear();
            _openedElements.AddRange(_elements.Skip(1));
        }
        
        private IEnumerable<(Sprite sprite, string name)> LoadSprites()
        {
            var loadedSprites = Resources.LoadAll("Sprites/Elements", typeof(Sprite));
            return loadedSprites.Select(s => ((Sprite) s, s.name));
        }
    }
}