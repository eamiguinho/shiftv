using System.Linq;
using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class UserProfileDtoFactory
    {
        public static IUserProfile Create(UserProfileDto dto)
        {
            if (dto == null) return null;
            var userProfile = Ioc.Container.Resolve<IUserProfile>();
            userProfile.About = dto.About;
            userProfile.Age = dto.Age;
            userProfile.Avatar = dto.Avatar;
            userProfile.FullName = dto.FullName;
            userProfile.Gender = dto.Gender;
            userProfile.IsVip = dto.IsVip;
            userProfile.Joined = dto.Joined;
            userProfile.LastLogin = dto.LastLogin;
            userProfile.Protected = dto.Protected;
            userProfile.Location = dto.Location;
            userProfile.Url = dto.Url;
            userProfile.Username = dto.Username;
            if (dto.Stats != null) userProfile.Stats = UserProfileStatsDtoFactory.Create(dto.Stats);
            //if (dto.Watching != null) userProfile.Watching = dto.Watching.Select(UserProfileWatchingDtoFactory.Create).ToList();
            if (dto.Watched != null)
                userProfile.Watched = dto.Watched.Select(UserProfileWatchedDtoFactory.Create).ToList();
            return userProfile;
        }

        public static UserProfileDto GetDto(IUserProfile userProfile)
        {
            if (userProfile == null) return null;
            var dto = new UserProfileDto
            {
                About = userProfile.About,
                Age = userProfile.Age,
                Avatar = userProfile.Avatar,
                FullName = userProfile.FullName,
                Gender = userProfile.Gender,
                IsVip = userProfile.IsVip,
                Joined = userProfile.Joined,
                LastLogin = userProfile.LastLogin,
                Location = userProfile.Location,
                Url = userProfile.Url,
                Username = userProfile.Username
            };
            return dto;
        }
    }
}