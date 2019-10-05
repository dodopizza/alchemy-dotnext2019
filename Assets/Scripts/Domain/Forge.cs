using System.Threading.Tasks;

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
                _getResultTask = MixElements(_firstElement, element);
            }
        }

        public void Clear()
        {
            _firstElement = null;
            _getResultTask = null;
        }

        private async Task<MixResult> MixElements(Element firstElement, Element secondElement)
        {
            var previouslyOpenedElement = _book.TryGetPreviousResult(firstElement.Id, secondElement.Id);
            
            if (previouslyOpenedElement != null)
            {
                _firstElement = null;
                return MixResult.Success(false, previouslyOpenedElement);
            }

            var checkResult = await _mixChecker.Check(firstElement.Id, secondElement.Id);

            if (checkResult.IsSuccess)
            {
                var element = _book.SaveNewReceipt(
                    firstElement.Id, 
                    secondElement.Id, 
                    checkResult.CreatedElementId);
                
                _firstElement = null;
                return MixResult.Success(true, element);
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