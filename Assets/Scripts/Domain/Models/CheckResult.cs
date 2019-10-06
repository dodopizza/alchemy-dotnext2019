using System;

namespace Domain.Models
{
    public class CheckResult
    {
        public bool IsSuccess { get; }
        
        public Guid CreatedElementId { get; }
        
        public int Scores { get; }

        private CheckResult(bool isSuccess, Guid createdElementId, int scores)
        {
            IsSuccess = isSuccess;
            CreatedElementId = createdElementId;
            Scores = scores;
        }

        public static CheckResult Success(string createdElementId, int scores)
        {
            return new CheckResult(true, Guid.Parse(createdElementId), scores);
        }

        public static CheckResult Failure()
        {
            return new CheckResult(false, Guid.Empty, 0);
        }
    }
}