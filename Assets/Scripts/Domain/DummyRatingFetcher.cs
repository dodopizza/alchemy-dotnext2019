using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain
{
	public class DummyRatingFetcher : IRatingFetcher
	{
		public Task<OperationResult<Rating>> GetRating()
		{
			return Task.FromResult(OperationResult<Rating>.Success(new Rating(new[]
			{
				new RatingEntry("Player1", 34), 
				new RatingEntry("Player2", 32), 
				new RatingEntry("Player3", 30), 
				new RatingEntry("Player4", 28), 
				new RatingEntry("Player5", 26), 
				new RatingEntry("Player6", 24), 
				new RatingEntry("Player7", 22), 
				new RatingEntry("Player8", 20) 
			}, Random.Range(10, 40))));
		}
	}
}