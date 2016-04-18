using System.Collections.Generic;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;

namespace ShiftvAPI.Contracts.Services.Lists
{
    public interface IListService
    {
        Task<DataResult<TraktList>> GetList(string username, string id);
        Task<DataResult<List<MiniShow>>> GetListShow(string username, string id);
        Task<DataResult<List<MiniMovie>>> GetListMovie(string username, string id);
    }
}