using Autofac;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class ShiftvUserStatsDtoFactory
    {
        public static IShiftvUserStats Create(ShiftvUserStatsDto dto)
        {
            if (dto == null) return null;
            var userProfile = Ioc.Container.Resolve<IShiftvUserStats>();
            userProfile.IsGold = dto.IsGold;
            userProfile.IsSilver = dto.IsSilver;
            userProfile.Username = dto.Username;
            return userProfile;
        }
    }
}