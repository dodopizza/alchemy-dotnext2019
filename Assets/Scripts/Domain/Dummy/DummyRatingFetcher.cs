using System.Threading.Tasks;
using Domain.Models;
using UnityEngine;

namespace Domain.Dummy
{
	public class DummyRatingFetcher : Interfaces.IRatingFetcher
	{
		public Task<OperationResult<Models.RatingEntry[]>> GetTopRating()
		{
			return Task.FromResult(OperationResult<Models.RatingEntry[]>.Success(new[]
			{
				new Models.RatingEntry("Player1", 34, 5, 1), 
				new Models.RatingEntry("Player2", 32, 5, 2), 
				new Models.RatingEntry("Player3", 30, 5, 3), 
				new Models.RatingEntry("Player4", 28, 5, 4), 
				new Models.RatingEntry("Player5", 26, 5, 5), 
				new Models.RatingEntry("Player6", 24, 5, 6), 
				new Models.RatingEntry("Player7", 22, 5, 7), 
				new Models.RatingEntry("Player8", 20, 5,8) 
			}));
		}

		public Task<OperationResult<Models.RatingEntry>> GetMyRating()
		{
			return Task.FromResult(
				OperationResult<Models.RatingEntry>.Success(new Models.RatingEntry("Eranikid", 30, 5, Random.Range(2, 15))));
		}
	}
}