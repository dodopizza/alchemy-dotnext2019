using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain
{
	public class DummyRatingFetcher : IRatingFetcher
	{
		public Task<OperationResult<RatingEntry[]>> GetTopRating()
		{
			return Task.FromResult(OperationResult<RatingEntry[]>.Success(new[]
			{
				new RatingEntry("Player1", 34, 1), 
				new RatingEntry("Player2", 32, 2), 
				new RatingEntry("Player3", 30, 3), 
				new RatingEntry("Player4", 28, 4), 
				new RatingEntry("Player5", 26, 5), 
				new RatingEntry("Player6", 24, 6), 
				new RatingEntry("Player7", 22, 7), 
				new RatingEntry("Player8", 20,8) 
			}));
		}

		public Task<OperationResult<RatingEntry>> GetMyRating()
		{
			return Task.FromResult(
				OperationResult<RatingEntry>.Success(new RatingEntry("You", 30, Random.Range(2, 15))));
		}
	}
}