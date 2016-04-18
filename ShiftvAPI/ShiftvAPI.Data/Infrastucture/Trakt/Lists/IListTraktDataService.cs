using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Lists
{
    public interface IListTraktDataService
    {
        Task<TraktList> GetListInfo(string username, string id);
        Task<List<TraktListItem>> GetListItems(string username, string id);
    }
}