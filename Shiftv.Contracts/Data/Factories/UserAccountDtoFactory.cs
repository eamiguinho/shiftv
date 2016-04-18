using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class UserAccountDtoFactory
    {
        public static IUserAccount Create(UserAccountDto dto)
        {
            if (dto == null) return null;
            var userAccount = Ioc.Container.Resolve<IUserAccount>();
            userAccount.AccountDetails = AccountDetailsDtoFactory.Create(dto.AccountDetails);
            userAccount.Message = dto.Message;
            userAccount.PasswordEnc = dto.PasswordEnc;
            userAccount.Status = dto.Status;
            userAccount.UserProfile = UserProfileDtoFactory.Create(dto.UserProfile);
            userAccount.UserSharingText = UserSharingTextDtoFactory.Create(dto.UserSharingText);
            userAccount.Username = dto.Username;
            return userAccount;
        }

        public static UserAccountDto GetDto(IUserAccount userAccount)
        {
            if (userAccount == null) return null;
            var dto = new UserAccountDto
            {
                AccountDetails = AccountDetailsDtoFactory.GetDto(userAccount.AccountDetails),
                Message = userAccount.Message,
                PasswordEnc = userAccount.PasswordEnc,
                Status = userAccount.Status,
                UserProfile = UserProfileDtoFactory.GetDto(userAccount.UserProfile),
                UserSharingText = UserSharingTextDtoFactory.GetDto(userAccount.UserSharingText),
                Username = userAccount.Username
            };
            return dto;
        }
    }
}