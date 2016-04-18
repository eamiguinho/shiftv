using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Categories
{
    public interface ICategoryTraktQueryService
    {
        Task<string> GetAllForMovies();
        Task<string> GetAllForShows();
    }
}