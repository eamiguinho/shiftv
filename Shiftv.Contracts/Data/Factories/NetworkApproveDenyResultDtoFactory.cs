using Autofac;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class NetworkApproveDenyResultDtoFactory
    {
        public static INetworkApproveDenyResult Create(NetworkApproveDenyResultDto networkApproveDenyResult)
        {
            var x = Ioc.Container.Resolve<INetworkApproveDenyResult>();
            x.Message = networkApproveDenyResult.Message;
            switch (networkApproveDenyResult.Status)
            {
                case "success":
                    x.Status = RequestResults.Success;
                    break;
                default:
                    x.Status = RequestResults.Failure;
                    break;
            }
            return x;
        }
    }
}