using Autofac;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class NetworkFollowResultDtoFactory
    {
        public static INetworkFollowResult Create(NetworkFollowResultDto networkfollowResult)
        {
            var x = Ioc.Container.Resolve<INetworkFollowResult>();
            x.Message = networkfollowResult.Message;
            switch (networkfollowResult.Status)
            {
                case "success":
                    x.Status = RequestResults.Success;
                    break;
                default:
                    x.Status = RequestResults.Failure;
                    break;
            }
            x.IsPending = networkfollowResult.IsPending;
            return x;
        }
    }
}