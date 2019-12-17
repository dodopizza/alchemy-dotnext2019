using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Domain
{
    internal class RecipeBook : IRecipeBook
    {
        private readonly IBaseElementsLoader _baseElementsLoader;
        private Dictionary<Guid, Element> _openedElements;
        private readonly Dictionary<(Guid firstId, Guid secondId), Guid> _openedRecipes;

        public RecipeBook(IBaseElementsLoader baseElementsLoader)
        {
            _baseElementsLoader = baseElementsLoader;
            _openedRecipes = Persistence.LoadRecipes();
        }
        
        public async Task<OperationResult<IEnumerable<Element>>> LoadInitialElements()
        {
            _openedElements = Persistence.LoadElements();

            if (_openedElements.Count == 0)
            {
                var getInitialElements = await _baseElementsLoader.GetBaseElements();

                if (getInitialElements.IsSuccess)
                {
                    _openedElements = getInitialElements.Data.ToDictionary(e => e.Id, e => e);
                    Persistence.SaveElements(_openedElements);
                }
                else
                {
                    return OperationResult<IEnumerable<Element>>.Failure();
                }
            }

            return OperationResult<IEnumerable<Element>>.Success(_openedElements.Values);
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