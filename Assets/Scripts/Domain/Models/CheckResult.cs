using System;

namespace Domain.Models
{
    public class CheckResult
    {
        public bool IsSuccess { get; }
        
        public Guid CreatedElementId { get; }
        
        public int Scores { get; }
        
        public string Description { get; }

        private CheckResult(bool isSuccess, Guid createdElementId, int scores, string description)
        {
            IsSuccess = isSuccess;
            CreatedElementId = createdElementId;
            Scores = scores;
            Description = description;
        }

        public static CheckResult Success(string createdElementId, int scores, string description)
        {
            return new CheckResult(true, Guid.Parse(createdElementId), scores, description);
        }

        public static CheckResult Failure()
        {
            return new CheckResult(false, Guid.Empty, 0, null);
        }
    }
}