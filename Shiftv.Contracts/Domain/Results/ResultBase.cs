namespace Shiftv.Contracts.Domain.Results
{
    public class ResultBase
    {
        public enum Results
        {
            Ok,
            Error,
            NoInternetConnection,
            Unauthorized
        }

        public Results Result { get; set; }
        public bool IsOk { get { return Result == Results.Ok; } }

    }
}