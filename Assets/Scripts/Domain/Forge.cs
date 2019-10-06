using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
    internal class Forge : IForge
    {
        private Element _firstElement;
        private Task<MixResult> _getResultTask;
        private readonly IReceiptsBook _book;
        private readonly IMixChecker _mixChecker;

        public Forge(IReceiptsBook book, IMixChecker mixChecker)
        {
            _book = book;
            _mixChecker = mixChecker;
        }

        public void AddElement(Element element)
        {
            if (_firstElement == null)
            {
                _firstElement = element;
            }
            else
            {
                _getResultTask = MixElements(_firstElement.Id, element.Id);
            }
        }

        public void Clear()
        {
            _firstElement = null;
            _getResultTask = null;
        }

        private async Task<MixResult> MixElements(Guid firstId, Guid secondId)
        {
            if (_book.TryGetPreviousResult(firstId, secondId, out var findPreviousResult))
            {
                return ReturnResult(findPreviousResult, false);
            }
            
            var operationResult = await _mixChecker.Check(firstId, secondId);

            if (operationResult.IsSuccess)
            {
                var checkResult = operationResult.Data;
                var saveNewResult = _book.SaveNewReceipt(
                    firstId,
                    secondId,
                    checkResult.CreatedElementId,
                    checkResult.IsSuccess);

                return ReturnResult(saveNewResult, true);
            }
            else
                throw new InvalidOperationException("Что-то пошло не так");
        }

        private MixResult ReturnResult(Element element, bool isNewlyCreated)
        {
            if (element != null)
            {
                _firstElement = null;
                return MixResult.Success(isNewlyCreated, element);
            }
            
            return MixResult.Fail();
        }

        public Task<MixResult> GetMixResult()
        {
            var result = _getResultTask;
            _getResultTask = null;
            return result;
        }
    }
}