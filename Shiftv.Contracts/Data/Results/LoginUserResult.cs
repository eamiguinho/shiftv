using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Contracts.Data.Results
{
    public class LoginUserResult : ResultBase
    {
        public string Message { get; set; }
        public static LoginUserResult Ok()
        {
            return new LoginUserResult { Result = Results.Ok };
        }

        public static LoginUserResult Error(string message)
        {
            return new LoginUserResult { Result = Results.Error, Message = message };
        }
        public static LoginUserResult Offline()
        {
            return new LoginUserResult { Result = Results.NoInternetConnection };
        }
        public static LoginUserResult Unauthorized()
        {
            return new LoginUserResult { Result = Results.Unauthorized };
        }
    }
}
