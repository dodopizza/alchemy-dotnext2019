using System;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
    internal class Forge : IForge
    {
        private Element _firstElement;
        private Task<OperationResult<MixResult>> _getResultTask;
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

        private async Task<OperationResult<MixResult>> MixElements(Guid firstId, Guid secondId)
        {
            if (_book.TryGetPreviousResult(firstId, secondId, out var findPreviousResult))
            {
                return OperationResult<MixResult>.Success(ReturnResult(findPreviousResult, false));
            }
            
            var operationResult = await _mixChecker.Check(firstId, secondId);

            if (operationResult.IsSuccess)
            {
                var checkResult = operationResult.Data;
                _book.SaveAttempt(firstId, secondId);

                if (!checkResult.IsSuccess)
                {
                    return OperationResult<MixResult>.Success(MixResult.Fail());
                }

                var saveNewResult = _book.SaveNewRecipe(
                    firstId,
                    secondId,
                    checkResult.CreatedElementId,
                    out var isNewlyCreated);

                // scores
                // description
                return OperationResult<MixResult>.Success(ReturnResult(saveNewResult, isNewlyCreated));
            }
            
            return OperationResult<MixResult>.Failure();
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

        public Task<OperationResult<MixResult>> GetMixResult()
        {
            var result = _getResultTask;
            _getResultTask = null;
            return result;
        }
    }
}