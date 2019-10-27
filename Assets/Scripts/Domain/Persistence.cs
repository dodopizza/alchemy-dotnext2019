using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.Models;
using UnityEngine;
using Utils;

namespace Domain
{
    public static class Persistence
    {
        private const string OpenedRecipesFileName = "openedRecipes.bin";
        private const string OpenedElementsFileName = "openedElements.bin";

        public static void EraseData()
        {
            File.Delete(Path.Combine(Application.persistentDataPath, OpenedElementsFileName));
            File.Delete(Path.Combine(Application.persistentDataPath, OpenedRecipesFileName));
        }
        
        public static void SaveElements(Dictionary<Guid, Element> elements)
        {
            var serialized = new ElementsDictionary
            {
                dictionary = elements.ToDictionary(x => x.Key.ToString(), x => x.Value.ToSerializable())
            };
            Save(serialized, OpenedElementsFileName);
        }

        public static void SaveRecipes(Dictionary<(Guid, Guid), Guid> recipes)
        {
            var serialized = new RecipesDictionary
            {
                dictionary = recipes.ToDictionary(x => new IdsPair
                    {
                        id1 = x.Key.Item1.ToString(),
                        id2 = x.Key.Item2.ToString()
                    }, 
                    x => x.Value.ToString())
            };
            Save(serialized, OpenedRecipesFileName);
        }
        
        public static Dictionary<(Guid, Guid), Guid> LoadRecipes()
        {
            var openedRecipes = new Dictionary<(Guid, Guid), Guid>();
            
            if (File.Exists(Path.Combine(Application.persistentDataPath, OpenedRecipesFileName)))
            {
                var serializableRecipes = Load<RecipesDictionary>(OpenedRecipesFileName);
                openedRecipes = serializableRecipes?.dictionary?
                    .ToDictionary(s => (new Guid(s.Key.id1), new Guid(s.Key.id2)), s => new Guid(s.Value));
            }
            
            return openedRecipes;
        }
        
        public static Dictionary<Guid, Element> LoadElements()
        {
            var openedElements = new Dictionary<Guid, Element>();

            if (File.Exists(Path.Combine(Application.persistentDataPath, OpenedElementsFileName)))
            {
                var serializableElements = Load<ElementsDictionary>(OpenedElementsFileName);
                openedElements = serializableElements?.dictionary?.ToDictionary(
                                     s => new Guid(s.Key), s => s.Value.ToElement())
                                  ?? new Dictionary<Guid, Element>();
            }

            return openedElements;
        }
        
        private static void Save<T>(T dictionary, string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            var x = JsonUtility.ToJson(dictionary);
            File.WriteAllText(filePath, JsonUtility.ToJson(dictionary));
        }
        
        private static T Load<T>(string fileName)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            var dictionary = JsonUtility.FromJson<T>(File.ReadAllText(filePath));
            return dictionary;
        }

        private class RecipesDictionary : SerializableDictionary<IdsPair, string>
        {}

        [Serializable]
        private class IdsPair
        {
            public string id1;
            public string id2;
        }
        
        private class ElementsDictionary : SerializableDictionary<string, SerializableElement>
        {}
        
        [Serializable]
        private class SerializableElement
        {
            public string id;

            public string sprite;

            public string name;

            public int scores;

            public string description;
        }
        
        private static SerializableElement ToSerializable(this Element element)
        {
            return new SerializableElement
            {
                id = element.Id.ToString(),
                description = element.Description,
                name = element.Name,
                scores = element.Score,
                sprite = element.SpriteName
            };
        }

        private static Element ToElement(this SerializableElement element)
        {
            return new Element(new Guid(element.id), element.sprite, element.name, element.scores, element.description);
        }
    }
}