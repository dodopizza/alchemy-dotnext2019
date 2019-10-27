namespace Domain.Models
{
    public class PlayerInfo
    {
        public PlayerInfo(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public string Name { get; }
        
        public int Score { get; }
    }
}