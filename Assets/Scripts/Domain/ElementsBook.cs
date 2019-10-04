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

        const string key = "OpenedElements";

        public ElementsBook()
        {
            LoadElements();
        }

        public IEnumerable<Element> GetOpenedElements()
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

                var value = string.Join(";", _openedElements.Select(x => x.Id.ToString()));
                PlayerPrefs.SetString(key, value);
                PlayerPrefs.Save();

                return true;
            }

            return false;
        }

        private void LoadElements()
        {
            _elements.Clear();
            var elementsText = (TextAsset) Resources.Load("Elements");
            _elements.AddRange(elementsText.text
                .Split('\n')
                .Select(x =>
                {
                    var a = x.Split(';');
                    return new Element(
                        new Guid(a[0]),
                        (Sprite) Resources.Load($"Sprites/Elements/{a[1]}", typeof(Sprite)),
                        a[2]);
                }));


            _openedElements.Clear();
            if (PlayerPrefs.HasKey(key))
            {
                var openedElementsString = PlayerPrefs.GetString(key);
                var openedElementsGuids = openedElementsString.Split(';').Select(x => new Guid(x));
                _openedElements.AddRange(_elements.Where(x => openedElementsGuids.Contains(x.Id)));
            }
            else
            {
                _openedElements.AddRange(_elements.Skip(2).Take(5));
            }
        }

        private IEnumerable<(Sprite sprite, string name)> LoadSprites()
        {
            var loadedSprites = Resources.LoadAll("Sprites/Elements", typeof(Sprite));
            return loadedSprites.Select(s => ((Sprite) s, s.name));
        }
    }
}