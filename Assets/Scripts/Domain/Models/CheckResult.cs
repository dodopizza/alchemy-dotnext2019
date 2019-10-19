using System;

namespace Domain.Models
{
    public class CheckResult
    {
        public bool IsSuccess { get; }
        
        public Element CreatedElement { get; }

        private CheckResult(bool isSuccess, Element element)
        {
            IsSuccess = isSuccess;
            CreatedElement = element;
        }

        public static CheckResult Success(
            string createdElementId,
            string spriteName,
            string name,
            int score,
            string description)
        {
            var element = new Element(Guid.Parse(createdElementId), spriteName, name, score, description);
            return new CheckResult(true, element);
        }

        public static CheckResult Failure()
        {
            return new CheckResult(false, null);
        }
    }
}