using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
	public interface IRatingFetcher
	{
		Task<OperationResult<RatingEntry[]>> GetTopRating();
		Task<OperationResult<RatingEntry>> GetMyRating();
	}
}