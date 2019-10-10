using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
	public interface IRatingFetcher
	{
		Task<OperationResult<Models.RatingEntry[]>> GetTopRating();
		Task<OperationResult<Models.RatingEntry>> GetMyRating();
	}
}