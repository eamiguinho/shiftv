using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Categories;
using Shiftv.Contracts.DataServices.Categories;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Categories
{
    public class CategoryTraktDataService : ICategoryTraktDataService
    {
        private ICategoryTraktQueryService _queryService;

        public CategoryTraktDataService(ICategoryTraktQueryService queryService)
        {
            _queryService = queryService;
        }
        public Task<List<ICategory>> GetAllForMovies()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetAllForMovies();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<CategoryDto>>(url);
                    return x.Select(CategoryDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<ICategory>> GetAllForShows()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetAllForShows();
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<CategoryDto>>(url);
                    return x.Select(CategoryDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}