using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.DataServices.Sync;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Sync;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Sync
{
    class SyncService : ISyncService
    {
        private readonly IUserService _userService;
        private readonly ISyncTraktDataService _syncTraktDataService;

        public SyncService(IUserService userService, ISyncTraktDataService syncTraktDataService)
        {
            _userService = userService;
            _syncTraktDataService = syncTraktDataService;
        }

        public async Task<DataResult> SyncWatchedShows()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.SyncWatchedShows(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> SyncWatchedMovies()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.SyncWatchedMovies(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> SyncShowRatings()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.SyncShowRatings(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> SyncSeasonRatings()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.SyncSeasonRatings(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> SyncEpisodeRatings()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.SyncEpisodeRatings(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> SyncMovieRatings()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.SyncMovieRatings(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> UploadRatingsToTrakt()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.UploadRatingsToTrakt(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> UploadWatchedEpisodesToTrakt()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.UploadWatchedEpisodesToTrakt(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> UploadCommentsToTrakt()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _syncTraktDataService.UploadCommentsToTrakt(currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == false)
                return new DataResult(StandardResults.Error);
            return new DataResult(StandardResults.Ok);
        }
    }
}