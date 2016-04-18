using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Categories;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Contracts.Services.Categories;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Categories
{
    class CategoryService : ServiceHelper, ICategoryService
    {
        private ICategoryTraktDataService _traktDataService;

        public CategoryService(ICategoryTraktDataService traktDataService)
        {
            _traktDataService = traktDataService;
        }

        public async Task<DataResult<List<ICategory>>> GetAllForMovies()
        {
            //if (!await IsInternet()) return new DataResult<List<ICategory>>(StandardResults.Offline);
            var req = await _traktDataService.GetAllForMovies();
            return req == null ? new DataResult<List<ICategory>>(StandardResults.Error) : new DataResult<List<ICategory>>(req);
        }

        public async Task<DataResult<List<ICategory>>> GetAllForShows()
        {
            //if (!await IsInternet()) return new DataResult<List<ICategory>>(StandardResults.Offline);
            var req = await _traktDataService.GetAllForShows();
            return req == null ? new DataResult<List<ICategory>>(StandardResults.Error) : new DataResult<List<ICategory>>(req);
        }
    }
}
