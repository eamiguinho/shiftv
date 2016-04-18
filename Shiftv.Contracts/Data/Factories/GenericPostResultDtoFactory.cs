using Autofac;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class GenericPostResultDtoFactory
    {
        public static IGenericPostResult Create(GenericPostResultDto genericPostResultDto)
        {
            var genericPost = Ioc.Container.Resolve<IGenericPostResult>();
            genericPost.Message = genericPostResultDto.Message;
            switch (genericPostResultDto.Status)
            {
                case "success":
                    genericPost.Status = RequestResults.Success;
                    break;
                default:
                    genericPost.Status = RequestResults.Failure;
                    break;
            }
            return genericPost;
        }
    }
}