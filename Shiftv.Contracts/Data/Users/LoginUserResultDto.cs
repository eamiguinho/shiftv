using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Contracts.Data.Users
{
    public class LoginUserResultDto :ResultBase
    {
        public static LoginUserResultDto Ok()
        {
            return new LoginUserResultDto { Result = Results.Ok };
        }

        public static LoginUserResultDto Error()
        {
            return new LoginUserResultDto { Result = Results.Error };
        }
        public static LoginUserResultDto Offline()
        {
            return new LoginUserResultDto { Result = Results.NoInternetConnection };
        }
    }
}