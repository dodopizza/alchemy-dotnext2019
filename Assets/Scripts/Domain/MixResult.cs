namespace Domain
{
    class MixResult
    {
        public bool Success { get; set; }
        
        public bool IsNewlyCreated { get; set; }
        
        public Element Element { get; set; }
    }
}