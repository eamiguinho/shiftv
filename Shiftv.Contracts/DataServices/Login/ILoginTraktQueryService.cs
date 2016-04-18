using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Login
{
    public interface ILoginTraktQueryService
    {
        Task<string> GetUserQuery();
        Task<string> GetLoginTest();
        Task<string> GetCreateAccount();
    }
}