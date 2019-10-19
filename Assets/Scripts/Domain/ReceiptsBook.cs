using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Domain.Models;
using UnityEngine;

namespace Domain
{
    class ReceiptsBook : IReceiptsBook
    {
        private Dictionary<Guid, Element> _openedElements = new Dictionary<Guid, Element>();
        private Dictionary<(Guid firstId, Guid secondId), Guid> _openedRecipes 
            = new Dictionary<(Guid firstId, Guid secondId), Guid>();

        private const string OpenedRecipesFileName = "openedRecipes.bin";
        private const string OpenedElementsFileName = "openedElements.bin";
        
        public ReceiptsBook()
        {
            LoadElements();
        }

        public IEnumerable<Element> GetOpenedElements()
        {
            return _openedElements.Values;
        }
        
        public bool TryGetPreviousResult(Guid firstElementId, Guid secondElementId, out Element element)
        {
            if (_openedRecipes.TryGetValue((firstElementId, secondElementId), out var resultId))
            {
                element = resultId == Guid.Empty ? null : _openedElements[resultId];
                return true;
            }

            element = null;
            return false;
        }

        public bool CheckAndSaveNewRecipe(Guid firstElementId, Guid secondElementId, Element createdElement)
        {
            _openedRecipes[(firstElementId, secondElementId)] = createdElement.Id;
            _openedRecipes[(secondElementId, firstElementId)] = createdElement.Id;

            var newlyCreated = !_openedElements.ContainsKey(createdElement.Id); 
            if (newlyCreated)
            {
                _openedElements[createdElement.Id] = createdElement;
                Persist(_openedElements, OpenedElementsFileName);
            }
            
            Persist(_openedRecipes, OpenedRecipesFileName);
            return newlyCreated;
        }

        public void SaveAttempt(Guid firstElementId, Guid secondElementId)
        {
            _openedRecipes[(firstElementId, secondElementId)] = Guid.Empty;
            _openedRecipes[(secondElementId, firstElementId)] = Guid.Empty;
            Persist(_openedRecipes, OpenedRecipesFileName);
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

        private void LoadElements()
        {
            if (File.Exists(OpenedRecipesFileName))
            {
                _openedRecipes = LoadPersistentObject<Dictionary<(Guid, Guid), Guid>>(OpenedRecipesFileName);
            }

            if (File.Exists(OpenedElementsFileName))
            {
                _openedElements = LoadPersistentObject<Dictionary<Guid, Element>>(OpenedElementsFileName);
            }
            else
            {
                _openedElements.Add(
                    new Guid("959ba1ca-7239-4a42-8f30-b5de84396faa"), 
                    Element.StartElement(new Guid("959ba1ca-7239-4a42-8f30-b5de84396faa"), "единичка", "единичка"));
                
                _openedElements.Add(
                    new Guid("6452b48f-31fd-4c66-94df-cfff1174d9d6"), 
                    Element.StartElement(new Guid("6452b48f-31fd-4c66-94df-cfff1174d9d6"), "dodo", "додо"));
                
                _openedElements.Add(
                    new Guid("911e1853-3ac5-4cf1-a242-f69fce2840c6"), 
                    Element.StartElement(new Guid("911e1853-3ac5-4cf1-a242-f69fce2840c6"), "pizza", "пицца"));
                
                _openedElements.Add(
                    new Guid("7b92464e-d032-4890-a127-aa68889d1d4e"), 
                    Element.StartElement(new Guid("7b92464e-d032-4890-a127-aa68889d1d4e"), "человек", "человек"));
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