using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using LoginUserResult = Shiftv.Contracts.Data.Results.LoginUserResult;

namespace Shiftv.Contracts.DataServices.Login
{
    public interface ILoginTraktDataService
    {
        Task<LoginUserResult> LoginToTrakt(string username, string password);
       // Task<IUserAccount> GetUser(string username, string pass);
        Task<CreateUserResult> CreateAccount(string createUsername, string password, string email);
       // Task<IUserAccount> GetUserLocalData(string username, string pass);
    }
}