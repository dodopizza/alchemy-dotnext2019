using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Dummy
{
	public class DummyRatingFetcher : Interfaces.IRatingFetcher
	{
		public Task<OperationResult<RatingEntry[]>> GetTopRating()
		{
			return Task.FromResult(OperationResult<RatingEntry[]>.Success(new[]
			{
				new RatingEntry("Player1", 34, 5, 1), 
				new RatingEntry("Player2", 32, 5, 2), 
				new RatingEntry("Player3", 30, 5, 3), 
				new RatingEntry("Player4", 28, 5, 4), 
				new RatingEntry("Player5", 26, 5, 5), 
				new RatingEntry("Player6", 24, 5, 6), 
				new RatingEntry("Player7", 22, 5, 7), 
				new RatingEntry("Player8", 20, 5,8) 
			}));
		}

		public Task<OperationResult<PlayerInfo>> GetPlayerInfo()
		{
			return Task.FromResult(OperationResult<PlayerInfo>.Success(new PlayerInfo("Eranikid", 30)));
		}
	}
}