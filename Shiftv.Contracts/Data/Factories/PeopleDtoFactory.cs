using System.Collections.Generic;
using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Peoples;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class PeopleDtoFactory
    {
        public static IPeople Create(PeopleDto dto)
        {
            if (dto == null) return null;
            var myPeople = Ioc.Container.Resolve<IPeople>();
            myPeople.Cast = dto.Cast != null ? dto.Cast.Select(CastDtoFactory.Create).ToList() : null;
            myPeople.Crew = dto.Crew != null ? TeamDtoFactory.Create(dto.Crew) : null;
            return myPeople;
        }
    }

    public class TeamDtoFactory
    {
        public static ITeam Create(TeamDto dto)
        {
            if (dto == null) return null;
            var team = Ioc.Container.Resolve<ITeam>();
            team.Art = dto.Art != null ? dto.Art.Select(ArtDtoFactory.Create).ToList() : null;
            team.Camera = dto.Camera != null ? dto.Camera.Select(CameraDtoFactory.Create).ToList() : null;
            team.CostumeMakeUp = dto.CostumeMakeUp != null ? dto.CostumeMakeUp.Select(CostumeMakeUpDtoFactory.Create).ToList() : null;
            team.Crew = dto.Crew != null ? dto.Crew.Select(CrewDtoFactory.Create).ToList() : null;
            team.Directing = dto.Directing != null ? dto.Directing.Select(DirectingDtoFactory.Create).ToList() : null;
            team.Production = dto.Production != null ? dto.Production.Select(ProductionDtoFactory.Create).ToList() : null;
            team.Sound = dto.Sound != null ? dto.Sound.Select(SoundDtoFactory.Create).ToList() : null;
            team.Writing = dto.Writing != null ? dto.Writing.Select(WritingDtoFactory.Create).ToList() : null;
            return team;
        }
    }

    public class WritingDtoFactory
    {
        public static IWriting Create(WritingDto dto)
        {
            if (dto == null) return null;
            var directing = Ioc.Container.Resolve<IWriting>();
            directing.Job = dto.Job;
            directing.Person = PersonDtoFactory.Create(dto.Person);
            return directing;
        }
    }

    public class SoundDtoFactory
    {
        public static ISound Create(SoundDto dto)
        {
            if (dto == null) return null;
            var directing = Ioc.Container.Resolve<ISound>();
            directing.Job = dto.Job;
            directing.Person = PersonDtoFactory.Create(dto.Person);
            return directing;
        }
    }

    public class ProductionDtoFactory
    {
        public static IProduction Create(ProductionDto dto)
        {
            if (dto == null) return null;
            var directing = Ioc.Container.Resolve<IProduction>();
            directing.Job = dto.Job;
            directing.Person = PersonDtoFactory.Create(dto.Person);
            return directing;
        }
    }

    public class DirectingDtoFactory
    {
        public static IDirecting Create(DirectingDto dto)
        {
            if (dto == null) return null;
            var directing = Ioc.Container.Resolve<IDirecting>();
            directing.Job = dto.Job;
            directing.Person = PersonDtoFactory.Create(dto.Person);
            return directing;
        }
    }

    public class CrewDtoFactory 
    {
        public static ICrew Create(CrewDto dto)
        {
            if (dto == null) return null;
            var crew = Ioc.Container.Resolve<ICrew>();
            crew.Job = dto.Job;
            crew.Person = PersonDtoFactory.Create(dto.Person);
            return crew;
        }
    }

    public class CostumeMakeUpDtoFactory    
    {
        public static ICostumeMakeUp Create(CostumeMakeUpDto dto)
        {
            if (dto == null) return null;
            var art = Ioc.Container.Resolve<ICostumeMakeUp>();
            art.Job = dto.Job;
            art.Person = PersonDtoFactory.Create(dto.Person);
            return art;
        }
    }

    public class CameraDtoFactory   
    {
        public static ICamera Create(CameraDto dto)
        {

            if (dto == null) return null;
            var art = Ioc.Container.Resolve<ICamera>();
            art.Job = dto.Job;
            art.Person = PersonDtoFactory.Create(dto.Person);
            return art;
        }
    }

    public class ArtDtoFactory
    {
        public static IArt Create(ArtDto dto)
        {

            if (dto == null) return null;
            var art = Ioc.Container.Resolve<IArt>();
            art.Job = dto.Job;
            art.Person = PersonDtoFactory.Create(dto.Person);
            return art;
        }
    }

    public class CastDtoFactory
    {
        public static ICast Create(CastDto dto)
        {
            if (dto == null) return null;
            var cast = Ioc.Container.Resolve<ICast>();
            cast.Character = dto.Character;
            cast.Person = PersonDtoFactory.Create(dto.Person);
            return cast;
        }
    }

    public class PersonDtoFactory
    {
        public static IPerson Create(PersonDto dto)
        {
            if (dto == null) return null;
            var person = Ioc.Container.Resolve<IPerson>();
            person.Biography = dto.Biography;
            person.Birthday = dto.Birthday;
            person.Birthplace = dto.Birthplace;
            person.Death = dto.Death;
            person.Homepage = dto.Homepage;
            person.Ids = IdsDtoFactory.Create(dto.Ids);
            person.Images = ImageDtoFactory.Create(dto.Images);
            person.Name = dto.Name;
            return person;
        }
    }
}