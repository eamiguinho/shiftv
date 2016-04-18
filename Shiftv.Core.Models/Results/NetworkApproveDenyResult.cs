using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Core.Models.Results
{
    public class NetworkApproveDenyResult : INetworkApproveDenyResult
    {
        public RequestResults Status { get; set; }
        public string Message { get; set; }
    }
}