using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Shiftv
{
    public interface ILoginShiftvDataService
    {
        Task<string> SaveUserSettings(TokenResult userTraktInfo);
    }
}