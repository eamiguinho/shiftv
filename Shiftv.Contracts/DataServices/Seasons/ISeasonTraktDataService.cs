using System.Threading.Tasks;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Contracts.DataServices.Seasons
{
    public interface ISeasonTraktDataService
    {
        Task<IGenericPostResult> SetSeasonAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title,
            int year, int season);
    }
}