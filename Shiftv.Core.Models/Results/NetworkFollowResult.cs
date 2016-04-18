using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Core.Models.Results
{
    public class NetworkFollowResult : INetworkFollowResult
    {
        public RequestResults Status { get; set; }
        public string Message { get; set; }
        public bool IsPending { get; set; }
    }
}