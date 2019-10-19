using System;
using System.Collections.Generic;
using Domain.Models;

namespace Domain
{
    internal class RecipeBook : IRecipeBook
    {
        private readonly Dictionary<Guid, Element> _openedElements;
        private readonly Dictionary<(Guid firstId, Guid secondId), Guid> _openedRecipes;

        public RecipeBook()
        {
            _openedElements = Persistence.LoadElements();
            _openedRecipes = Persistence.LoadRecipes();
            
            if (_openedElements.Count == 0)
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
                Persistence.SaveElements(_openedElements);
            }
            
            Persistence.SaveRecipes(_openedRecipes);
            return newlyCreated;
        }

        public void SaveAttempt(Guid firstElementId, Guid secondElementId)
        {
            _openedRecipes[(firstElementId, secondElementId)] = Guid.Empty;
            _openedRecipes[(secondElementId, firstElementId)] = Guid.Empty;
            Persistence.SaveRecipes(_openedRecipes);
        }
    }
}