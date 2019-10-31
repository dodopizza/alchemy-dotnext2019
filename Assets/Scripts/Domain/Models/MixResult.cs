namespace Domain.Models
{
    class MixResult
    {
        private MixResult(bool isSuccess, bool isNewlyCreated, Element element, bool isCrash)
        {
            IsSuccess = isSuccess;
            IsNewlyCreated = isNewlyCreated;
            Element = element;
            IsCrash = isCrash;
        }

        public bool IsSuccess { get; }
        
        public bool IsNewlyCreated { get; }
        
        public Element Element { get; }
        
        public bool IsCrash { get; }
        
        public static MixResult Success(bool isNewlyCreated, Element element)
        {
            return new MixResult(true, isNewlyCreated, element, false);
        }

        public static MixResult Fail()
        {
            return new MixResult(false, false, null, false);
        }

        public static MixResult Crash()
        {
            return new MixResult(false, false, null, true);
        }
    }
}