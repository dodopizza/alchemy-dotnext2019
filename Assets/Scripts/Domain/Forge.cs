using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Domain
{
    class Forge : IForge
    {
        private Element _firstElement;
        private Task<MixResult> _getResultTask;
        private readonly IElementsBook _book;

        public Forge(IElementsBook book)
        {
            _book = book;
        }

        public void AddElement(Element element)
        {
            if (_firstElement == null)
            {
                _firstElement = element;
            }
            else
            {
                _getResultTask = MixElements(_firstElement, element);
            }
        }

        private Task<MixResult> MixElements(Element firstElement, Element secondElement)
        {
            if (Random.Range(0, 2) == 1)
            {
                _firstElement = null;
                var elementId = Guid.NewGuid();
                return Task.FromResult(new MixResult
                {
                    Success = true,
                    NewlyCreated = _book.ElementOpened(elementId),
                    Element = _book.OpenAndGetElement(elementId)
                });
                
            }

            return Task.FromResult(new MixResult
            {
                Success = false
            });
        }

        public Task<MixResult> GetMixResult()
        {
            var result = _getResultTask;
            _getResultTask = null;
            return result;
        }
    }
}