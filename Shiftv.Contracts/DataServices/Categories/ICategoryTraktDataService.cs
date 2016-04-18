using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Categories;

namespace Shiftv.Contracts.DataServices.Categories
{
    public interface ICategoryTraktDataService
    {
        Task<List<ICategory>> GetAllForMovies();
        Task<List<ICategory>> GetAllForShows();
    }
}