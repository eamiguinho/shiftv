using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;

namespace ShiftvAPI.Contracts.Infrastucture.Trakt.Login
{
    public interface ILoginTraktQueryService
    {
        Task<string> GetToken();
        Task<TokenRequest> GetTokenPost(string code);
        Task<string> GetUserSettings();
    }
}