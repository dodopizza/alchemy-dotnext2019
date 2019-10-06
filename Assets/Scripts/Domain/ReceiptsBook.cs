using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using UnityEngine;

namespace Domain
{
    class ReceiptsBook : IReceiptsBook
    {
        private Dictionary<Guid, Element> _elements = new Dictionary<Guid, Element>();
        private Dictionary<Guid, Element> _openedElements = new Dictionary<Guid, Element>();
        private readonly Dictionary<(Guid firstId, Guid secondId), Guid> _openedReceipts 
            = new Dictionary<(Guid firstId, Guid secondId), Guid>();
        private readonly HashSet<(Guid, Guid)> _attempts = new HashSet<(Guid, Guid)>();

        const string key = "OpenedElements";

        public ReceiptsBook()
        {
            LoadElements();
        }

        public IEnumerable<Element> GetOpenedElements()
        {
            return _openedElements.Values;
        }
        
        public Element SaveNewReceipt(Guid firstElementId, Guid secondElementId, Guid resultId, out bool newlyCreated)
        {
            _openedReceipts[(firstElementId, secondElementId)] = resultId;
            _openedReceipts[(secondElementId, firstElementId)] = resultId;

            var result = _elements[resultId];

            newlyCreated = !_openedElements.ContainsKey(resultId); 
            if (newlyCreated)
            {
                _openedElements[resultId] = result;
            }
            // todo: save
            return result;
        }

        public void SaveAttempt(Guid firstElementId, Guid secondElementId)
        {
            _attempts.Add((firstElementId, secondElementId));
            _attempts.Add((secondElementId, firstElementId));
        }

        public bool TryGetPreviousResult(Guid firstElementId, Guid secondElementId, out Element element)
        {
            if (_openedReceipts.TryGetValue((firstElementId, secondElementId), out var resultId))
            {
                element = _elements[resultId];
                return true;
            }

            element = null;
            return _attempts.Contains((firstElementId, secondElementId));
        }

        private void LoadElements()
        {
            _elements.Clear();
            var elementsText = (TextAsset) Resources.Load("Elements");
            var elementsList = elementsText.text
                .Split('\n')
                .Select(x =>
                {
                    var a = x.Split(';');
                    return new Element(
                        new Guid(a[0]),
                        (Sprite) Resources.Load($"Sprites/Elements/{a[1]}", typeof(Sprite)),
                        a[2]);
                }).ToList();

            _elements = elementsList.ToDictionary(e => e.Id, e => e);

            // todo: load from save
            foreach (var element in elementsList.Skip(1).Take(4))
            {
                _openedReceipts[(Guid.NewGuid(), Guid.NewGuid())] = element.Id;
                _openedElements[element.Id] = element;
            }
        }
    }
}