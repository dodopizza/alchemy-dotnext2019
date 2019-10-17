using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Domain.Models;
using UnityEngine;

namespace Domain
{
    class ReceiptsBook : IReceiptsBook
    {
        private Dictionary<Guid, Element> _elements = new Dictionary<Guid, Element>();
        private readonly Dictionary<Guid, Element> _openedElements = new Dictionary<Guid, Element>();
        private Dictionary<(Guid firstId, Guid secondId), Guid> _openedRecipes 
            = new Dictionary<(Guid firstId, Guid secondId), Guid>();
        private HashSet<(Guid, Guid)> _attempts = new HashSet<(Guid, Guid)>();

        private const string AttemptsFileName = "attempts.bin";
        private const string OpenedRecipesFileName = "openedRecipes.bin";

        const string key = "OpenedElements";
        
        public ReceiptsBook()
        {
            LoadElements();
        }

        public IEnumerable<Element> GetOpenedElements()
        {
            return _openedElements.Values;
        }
        
        public Element SaveNewRecipe(Guid firstElementId, Guid secondElementId, Guid resultId, out bool newlyCreated)
        {
            _openedRecipes[(firstElementId, secondElementId)] = resultId;
            _openedRecipes[(secondElementId, firstElementId)] = resultId;

            var result = _elements[resultId];

            newlyCreated = !_openedElements.ContainsKey(resultId); 
            if (newlyCreated)
            {
                _openedElements[resultId] = result;
            }
            
            Persist(_openedRecipes, OpenedRecipesFileName);
            return result;
        }

        public void SaveAttempt(Guid firstElementId, Guid secondElementId)
        {
            _attempts.Add((firstElementId, secondElementId));
            _attempts.Add((secondElementId, firstElementId));
            Persist(_attempts, AttemptsFileName);
        }

        private void Persist<T>(T persistentObject, string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            
            using (var stream = File.Create(filePath))
            {
                Debug.Log($"writing '{fileName}'");
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, persistentObject);
                stream.Close();
            }
        }
        
        public bool TryGetPreviousResult(Guid firstElementId, Guid secondElementId, out Element element)
        {
            if (_openedRecipes.TryGetValue((firstElementId, secondElementId), out var resultId))
            {
                element = _elements[resultId];
                return true;
            }

            element = null;
            return _attempts.Contains((firstElementId, secondElementId));
        }

        private void LoadElements()
        {
            // todo: доделать персистентность
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

            _attempts = LoadPersistentObject<HashSet<(Guid, Guid)>>(AttemptsFileName);
            _openedRecipes = LoadPersistentObject<Dictionary<(Guid, Guid), Guid>>(OpenedRecipesFileName);
            
            // todo: load from save
            foreach (var element in elementsList.Skip(1).Take(4))
            {
                _openedRecipes[(Guid.NewGuid(), Guid.NewGuid())] = element.Id;
                _openedElements[element.Id] = element;
            }
        }
        
        private static T LoadPersistentObject<T>(string fileName) where T : new()
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(filePath))
            {
                return new T();
            }
            
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                Debug.Log($"reading from '{fileName}'");
                var formatter = new BinaryFormatter();
                var loaded = (T)formatter.Deserialize(stream);
                stream.Close();
                return loaded;
            }
        }
    }
}