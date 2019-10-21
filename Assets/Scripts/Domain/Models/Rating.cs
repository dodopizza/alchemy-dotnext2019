namespace Domain.Models
{
	public class RatingEntry
	{
		public RatingEntry(string nickname, int score, int elements, int position)
		{
			Nickname = nickname;
			Score = score;
			Elements = elements;
			Position = position;
		}
		
		public string Nickname { get; }
		public int Score { get; }
		public int Elements { get; }
		public int Position { get; }
	}
}