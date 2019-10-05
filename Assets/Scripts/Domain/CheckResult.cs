using System;

namespace Domain
{
    public class CheckResult
    {
        public bool IsSuccess { get; set; }
        
        public Guid CreatedElementId { get; set; }
        
        public int Scores { get; set; }
    }
}