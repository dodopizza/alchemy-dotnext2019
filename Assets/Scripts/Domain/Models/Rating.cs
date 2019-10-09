namespace Domain.Models
{
	public class RatingEntry
	{
		public RatingEntry(string nickname, int rating, int position)
		{
			Nickname = nickname;
			Rating = rating;
			Position = position;
		}
		
		public string Nickname { get; }
		public int Rating { get; }
		public int Position { get; }
	}
}