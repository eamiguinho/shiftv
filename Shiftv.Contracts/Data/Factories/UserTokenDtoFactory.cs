using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class UserTokenDtoFactory
    {
        public static IUserToken Create(UserTokenDto userTokenDto)
        {
            if (userTokenDto == null) return null;

            var userToken = Ioc.Container.Resolve<IUserToken>();
            userToken.AccessToken = userTokenDto.AccessToken;
            userToken.Error = userTokenDto.Error;
            userToken.ErrorDescription = userTokenDto.ErrorDescription;
            userToken.ExpiresIn = userTokenDto.ExpiresIn;
            userToken.RefreshToken = userTokenDto.RefreshToken;
            userToken.Scope = userTokenDto.Scope;
            userToken.TokenType = userTokenDto.TokenType;
            userToken.UserSettings = UserSettingsDtoFactory.Create(userTokenDto.UserSettings);
            userToken.ExpiresAt = userTokenDto.ExpiresAt;
            userToken.TraktAccessToken = userTokenDto.TraktAccessToken;
            return userToken;
        }

        public static UserTokenDto GetDto(IUserToken userToken)
        {
            if (userToken == null) return null;
            var dto = new UserTokenDto();
            dto.AccessToken = userToken.AccessToken;
            dto.Error = userToken.Error;
            dto.ErrorDescription = userToken.ErrorDescription;
            dto.ExpiresIn = userToken.ExpiresIn;
            dto.RefreshToken = userToken.RefreshToken;
            dto.Scope = userToken.Scope;
            dto.TokenType = userToken.TokenType;
            dto.UserSettings = UserSettingsDtoFactory.GetDto(userToken.UserSettings);
            dto.ExpiresAt = userToken.ExpiresAt;
            dto.TraktAccessToken = userToken.TraktAccessToken;
            return dto;
        }
    }

    public class UserSettingsDtoFactory
    {
        public static IUserSettings Create(UserSettingsDto dto)
        {
            if (dto == null) return null;
            var userSettings = Ioc.Container.Resolve<IUserSettings>();
            userSettings.Account = AccountDtoFactory.Create(dto.Account);
            userSettings.User = UserDtoFactory.Create(dto.User);
            return userSettings;
        }

        public static UserSettingsDto GetDto(IUserSettings userSettings)
        {
            if (userSettings == null) return null;
            var dto = new UserSettingsDto();
            dto.Account = AccountDtoFactory.GetDto(userSettings.Account);
            dto.User = UserDtoFactory.GetDto(userSettings.User);
            return dto;    
        }
    }
}
