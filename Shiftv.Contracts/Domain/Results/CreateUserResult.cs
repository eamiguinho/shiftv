namespace Shiftv.Contracts.Domain.Results
{
    public class CreateUserResult :ResultBase
    {
        public static CreateUserResult Ok()
        {
            return new CreateUserResult { Result = Results.Ok };
        }

        public static CreateUserResult Error(string error)
        {
            return new CreateUserResult { Result = Results.Error, ErrorMessage = error };
        } 
        public static CreateUserResult Error()
        {
            return new CreateUserResult { Result = Results.Error };
        }
        public static CreateUserResult Offline()
        {
            return new CreateUserResult { Result = Results.NoInternetConnection };
        }

        public string ErrorMessage { get; set; }
    }
}