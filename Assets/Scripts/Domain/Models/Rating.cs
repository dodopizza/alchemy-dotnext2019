namespace Domain.Models
{
	public class Rating
	{
		public Rating(RatingEntry[] top, RatingEntry own)
		{
			Top = top;
			Own = own;
		}

		public RatingEntry[] Top { get; }
		public RatingEntry Own { get; }
	}

	public class RatingEntry
	{
		public RatingEntry(string nickname, int rating)
		{
			Nickname = nickname;
			Rating = rating;
		}
		
		public string Nickname { get; }
		public int Rating { get; }
	}
}