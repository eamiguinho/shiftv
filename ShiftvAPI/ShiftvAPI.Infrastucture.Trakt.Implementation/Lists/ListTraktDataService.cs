using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Lists;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Lists
{
   public class ListTraktDataService : IListTraktDataService
    {
        private IListTraktQueryService _queryService;

        public ListTraktDataService(IListTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<TraktList> GetListInfo(string username, string id)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetListInfo(username,id);
                    TraktList x = await TraktDataServiceHelper.GetObjectWithoutCredentials<TraktList>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<List<TraktListItem>> GetListItems(string username, string id)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetListItems(username, id);
                    List<TraktListItem> x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<TraktListItem>>(url);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
