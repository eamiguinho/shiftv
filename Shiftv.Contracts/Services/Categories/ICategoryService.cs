using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Categories
{
    public interface ICategoryService
    {
        Task<DataResult<List<ICategory>>> GetAllForMovies();
        Task<DataResult<List<ICategory>>> GetAllForShows();
    }
}