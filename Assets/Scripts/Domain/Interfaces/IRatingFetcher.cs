using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
	public interface IRatingFetcher
	{
		Task<OperationResult<RatingEntry[]>> GetTopRating();
		Task<OperationResult<PlayerInfo>> GetPlayerInfo();
	}
}