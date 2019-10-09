namespace Domain.Models
{
	public class Rating
	{
		public Rating(RatingEntry[] top, int ownRating)
		{
			Top = top;
			OwnRatingRating = ownRating;
		}

		public RatingEntry[] Top { get; }
		public int OwnRatingRating { get; }
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