using System.Threading.Tasks;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.DataServices.Seasons;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Seasons;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Global;


namespace Shiftv.Services.Implementation.Seasons
{
    public class SeasonService : ServiceHelper, ISeasonService
    {
        private ISeasonTraktDataService _seasonDataService;
        private IUserService _userService;
        private IShowService _showService;

        public SeasonService(ISeasonTraktDataService seasonTraktData, IUserService userService, IShowService showService)
        {
            _seasonDataService = seasonTraktData;
            _userService = userService;
            _showService = showService;
        }

        public async Task<DataResult<IGenericPostResult>> SetSeasonAsSeen(int season)
        {
            if (season <= -1) return new DataResult<IGenericPostResult>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var show = _showService.GetCurrentShow();
            if (user == null || show == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var res = await _seasonDataService.SetSeasonAsSeen(UserTokenDtoFactory.GetDto(user), show.Ids.TvDbId.Value, show.Ids.ImdbId, show.Title, show.Year.Value, season);
            if (res == null || res.Status == RequestResults.Failure)
                return new DataResult<IGenericPostResult>(StandardResults.Error);
            _showService.UpdateCurrentShow();
            return new DataResult<IGenericPostResult>(res);
        }
    }
}
