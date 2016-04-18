using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;

namespace ShiftvAPI.Contracts.Services.Login
{
    public interface ILoginService
    {
        Task<DataResult<TokenResult>> GetToken(string code);
    }
}