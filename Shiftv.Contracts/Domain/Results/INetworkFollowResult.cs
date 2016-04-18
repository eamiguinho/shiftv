namespace Shiftv.Contracts.Domain.Results
{
    public interface INetworkFollowResult
    {
        RequestResults Status { get; set; }

        string Message { get; set; }

        bool IsPending { get; set; }
    }
}