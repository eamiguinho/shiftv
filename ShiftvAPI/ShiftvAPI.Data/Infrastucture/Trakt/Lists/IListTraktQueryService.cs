using System.Threading.Tasks;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Lists
{
    public interface IListTraktQueryService
    {
        Task<string> GetListInfo(string user, string id);
        Task<string> GetListItems(string user, string id);
    }
}