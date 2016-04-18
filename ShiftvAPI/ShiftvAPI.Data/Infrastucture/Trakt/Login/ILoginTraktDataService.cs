using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Login
{
    public interface ILoginTraktDataService
    {
        Task<TokenResult> GetToken(string code);
        Task<UserSettings> GetUserSettings(string accessToken);
    }
}