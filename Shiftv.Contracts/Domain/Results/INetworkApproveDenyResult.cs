namespace Shiftv.Contracts.Domain.Results
{
    public interface INetworkApproveDenyResult
    {
        RequestResults Status { get; set; }

        string Message { get; set; }
    }
}