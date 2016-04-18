namespace Shiftv.Contracts.Domain.Results
{
    public class CreateUserResultDto : ResultBase
    {
        public static CreateUserResultDto Ok()
        {
            return new CreateUserResultDto { Result = Results.Ok };
        }

        public static CreateUserResultDto Error(string error)
        {
            return new CreateUserResultDto { Result = Results.Error, ErrorMessage = error };
        }
        public static CreateUserResultDto Error()
        {
            return new CreateUserResultDto { Result = Results.Error };
        }
        public static CreateUserResultDto Offline()
        {
            return new CreateUserResultDto { Result = Results.NoInternetConnection };
        }

        public string ErrorMessage { get; set; }
    }
}