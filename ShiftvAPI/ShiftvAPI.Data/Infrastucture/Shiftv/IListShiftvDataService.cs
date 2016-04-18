using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Shiftv
{
    public interface IListShiftvDataService
    {
        Task<TraktList> GetList(string username, string id);
        Task SaveList(TraktList movieData, string username, string id);
    }
}