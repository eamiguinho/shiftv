using Autofac;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public class AccountDtoFactory
    {
        public static IAccount Create(AccountDto dto)
        {
            if (dto == null) return null;
            var accountDetails = Ioc.Container.Resolve<IAccount>();
            accountDetails.CoverImage = dto.CoverImage;
            accountDetails.Timezone = dto.Timezone;
            return accountDetails;
        }

        public static AccountDto GetDto(IAccount account)
        {
            if (account == null) return null;
            var dto = new AccountDto
            {
                CoverImage = account.CoverImage,
                Timezone = account.Timezone,
            };
            return dto;
        }
    }
}