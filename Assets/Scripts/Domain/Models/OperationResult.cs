namespace Domain.Models
{
    public class OperationResult<T> where T: class
    {
        public T Data { get; }
        
        public bool IsSuccess { get; }

        private OperationResult(T data, bool isSuccess)
        {
            Data = data;
            IsSuccess = isSuccess;
        }
        
        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>(data, true);
        }

        public static OperationResult<T> Failure()
        {
            return new OperationResult<T>(null, false);
        }
    }
}