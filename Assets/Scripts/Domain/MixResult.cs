namespace Domain
{
    class MixResult
    {
        private MixResult(bool isSuccess, bool isNewlyCreated, Element element)
        {
            IsSuccess = isSuccess;
            IsNewlyCreated = isNewlyCreated;
            Element = element;
        }

        public bool IsSuccess { get; }
        
        public bool IsNewlyCreated { get; }
        
        public Element Element { get; }

        public static MixResult Success(bool isNewlyCreated, Element element)
        {
            return new MixResult(true, isNewlyCreated, element);
        }

        public static MixResult Fail()
        {
            return new MixResult(false, false, null);
        }
    }
}