using Autofac;
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class ActorDtoFactory
    {
        public static IActor Create(ActorDto dto)
        {
            if (dto == null) return null;
            var actor = Ioc.Container.Resolve<IActor>();
            actor.Character = dto.Character;
            actor.Image = PeopleImageDtoFactory.Create(dto.Image);
            actor.Name = dto.Name;
            return actor;
        }
    }
}