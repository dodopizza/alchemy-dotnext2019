using System;

namespace Domain.Models
{
    public class CheckResult
    {
        public bool IsSuccess { get; set; }
        
        public Guid CreatedElementId { get; set; }
        
        public int Scores { get; set; }
    }
}