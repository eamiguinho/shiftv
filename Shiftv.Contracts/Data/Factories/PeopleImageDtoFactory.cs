using Autofac;
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class PeopleImageDtoFactory
    {
        public static IPeopleImage Create(PeopleImageDto dto)
        {
            if (dto == null) return null;
            var peopleimage = Ioc.Container.Resolve<IPeopleImage>();
            peopleimage.Headshot = dto.Headshot;
            return peopleimage;
        }
    }
}