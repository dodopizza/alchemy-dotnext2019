using System.Threading.Tasks;
using Domain.Models;

namespace Domain
{
	public interface IRatingFetcher
	{
		Task<OperationResult<Rating>> GetRating();
	}
}