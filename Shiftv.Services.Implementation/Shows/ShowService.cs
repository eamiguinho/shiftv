using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.DataServices.Shows;
using Shiftv.Contracts.Domain.Categories;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Accounts;
using Shiftv.Contracts.Services.Shows;
using Shiftv.Global;

namespace Shiftv.Services.Implementation.Shows
{
    public class ShowService : ServiceHelper, IShowService
    {
        private List<IMiniShow> _trending;
        private IShow _selectedSerie;
        private IShowTraktDataService _showDataService;
        private IUserService _userService;
        private List<IMiniShow> _popular;

        public ShowService(IShowTraktDataService dataService, IUserService userService)
        {
            _showDataService = dataService;
            _userService = userService;
        }


        public async Task<DataResult<List<IMiniShow>>> GetTrending(bool isLaunch = false)
        {
            if (_trending == null)
            {
                var currentUser = _userService.GetCurrentUser();
                var res = await _showDataService.GetTrending(UserTokenDtoFactory.GetDto(currentUser));

                if (res == null)
                    return new DataResult<List<IMiniShow>>(StandardResults.Error);

                if (!isLaunch) _trending = res;
                //HACK

                return new DataResult<List<IMiniShow>>(_trending);
            }
            return new DataResult<List<IMiniShow>>(_trending);
        }

        private async void LoadData(List<IShow> res)
        {
            CoreServices.User.SetUser(null);
            //var x = await Get();
            //if (x.IsOk && x.Data != null && x.Data.Count > 0)
            //{
            //    res = x.Data;
            //}
            //else return;
            foreach (var show in res)
            {
                try
                {
                    await SetCurrent(show);
                    var sFull = GetCurrentShow();
                    //if (sFull != null && sFull.Seasons != null && sFull.Seasons.Count > 0)
                    //{
                    //    Debug.WriteLine(sFull.Title + "synced");
                    //    foreach (var season in sFull.Seasons)
                    //    {
                    //        Debug.WriteLine(sFull.Title + "synced season" + season.Number);
                    //        var b = CoreServices.Episode.GetEpisodesBySeason(season.Number);
                    //    }
                    //}
                }
                catch (Exception e)
                {

                }
            }
        }

        public void UpdateTrending()
        {
            if (_trending == null) return;
            var currentShow = GetCurrentShow();
            if (currentShow == null) return;
            //if (_trending.Any(x => x.Ids.TraktId == currentShow.Ids.TraktId))
            //{
            //    var show = _trending.FirstOrDefault(x => x.Ids.TraktId == currentShow.Ids.TraktId);
            //    if (show == null) return;
       
            //    show.UserRating = currentShow.Rating;
            //    show.UserRating = currentShow.UserRating;
            //}
        }

        public async Task<DataResult<List<IMiniShow>>> GetTop()
        {
            if (!await IsInternet()) return new DataResult<List<IMiniShow>>(StandardResults.Offline);
            var list = await CoreServices.Show.GetPopular();
            return list.IsOk ? new DataResult<List<IMiniShow>>(list.Data.OrderByDescending(x => x.Rating).ToList()) : new DataResult<List<IMiniShow>>(StandardResults.Error);
        }

        public async Task<DataResult<List<IMiniShow>>> GetFresh()
        {
            //await Task.Delay(5000);
            //return new DataResult<List<IShow>>(StandardResults.Error);
            if (!await IsInternet()) return new DataResult<List<IMiniShow>>(StandardResults.Offline);
            var list = await GetTrending();
            var listPop = await GetPopular();
            var listTotal = new List<IMiniShow>();
            listTotal.AddRange(list.Data);
            listTotal.AddRange(listPop.Data);
            var finalList = new List<IMiniShow>();
            foreach (var miniShow in listTotal.Where(miniShow => finalList.Any(x => x.Ids.ImdbId == miniShow.Ids.ImdbId)))
            {
                finalList.Add(miniShow);
            }
            return list.IsOk ? new DataResult<List<IMiniShow>>(list.Data.OrderByDescending(x => x.FirstAiredData).ToList()) : new DataResult<List<IMiniShow>>(StandardResults.Error);
        }

        public async Task<DataResult<IShow>> GetByImdbId(IIds ids)
        {
            if (ids == null) return new DataResult<IShow>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<IShow>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var req = await _showDataService.GetByImdbId(UserTokenDtoFactory.GetDto(user), IdsDtoFactory.GetDto(ids));
            return req == null ? new DataResult<IShow>(StandardResults.Error) : new DataResult<IShow>(req);
        }


        public async Task<DataResult<IShow>> GetByImdbIdAzure(IIds ids)
        {
            if (ids == null) return new DataResult<IShow>(StandardResults.Error);
            var user = _userService.GetCurrentUser();
            var req = await _showDataService.GetByImdbId(UserTokenDtoFactory.GetDto(user), IdsDtoFactory.GetDto(ids));
            return req == null ? new DataResult<IShow>(StandardResults.Error) : new DataResult<IShow>(req);
        }

        public async Task<DataResult> SetCurrent(IMiniShow serie)
        {
            if (serie == null) return new DataResult(StandardResults.Error);
            var show = await GetByImdbId(serie.Ids);
            _selectedSerie = show.IsOk ? show.Data : null;
            //_selectedSerie = null;
            return show;
        }

        public async Task<DataResult> SetCurrent(IShow serie)
        {
            if (serie == null) return new DataResult(StandardResults.Error);
            var show = await GetByImdbId(serie.Ids);
            _selectedSerie = show.IsOk ? show.Data : null;
            //_selectedSerie = null;
            return show;
        }

        public async Task<DataResult> SetCurrentAzure(IShow serie)
        {
            if (serie == null) return new DataResult(StandardResults.Error);
            var show = await GetByImdbIdAzure(serie.Ids);
            _selectedSerie = show.IsOk ? show.Data : null;
            //_selectedSerie = null;
            return show;
        }

        public async Task<DataResult<IPeople>> GetPeople()
        {
            //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
            var currentShow = GetCurrentShow();
            var res = await _showDataService.GetPeople(IdsDtoFactory.GetDto(currentShow.Ids));
            if (res == null) return new DataResult<IPeople>(StandardResults.Error);
            return new DataResult<IPeople>(res);
        }

        public async Task<DataResult<List<IMiniShow>>> GetPopular()
        {
            if (_popular == null)
            {
                var currentUser = _userService.GetCurrentUser();
                var res = await _showDataService.GetPopular(UserTokenDtoFactory.GetDto(currentUser));

                if (res == null)
                    return new DataResult<List<IMiniShow>>(StandardResults.Error);

                _popular = res;
                //HACK

                return new DataResult<List<IMiniShow>>(_popular);
            }
            return new DataResult<List<IMiniShow>>(_popular);
        }

        public async Task<DataResult> ForceUpdate()
        {
            var currentShow = GetCurrentShow();
            if (currentShow == null) return new DataResult<List<IMiniShow>>(StandardResults.Error);
            var currentUser = _userService.GetCurrentUser();
            var res = await _showDataService.ForceUpdate(IdsDtoFactory.GetDto(currentShow.Ids), currentUser != null ? UserTokenDtoFactory.GetDto(currentUser) : null);
            if (res == null)
                return new DataResult<List<IMiniShow>>(StandardResults.Error);
            await SetCurrent(res);
            return new DataResult(StandardResults.Ok);
        }

        public async Task<DataResult> SetCurrent(IIds ids, string title)
        {
            var show = await GetByImdbId(ids);
            _selectedSerie = show.IsOk ? show.Data : null;
            _selectedSerie = null;
            return show;
        }


        public IShow GetCurrentShow()
        {
            return _selectedSerie;
        }

        public async Task<IShow> GetCurrentShowWithoutCache()
        {
            await SetCurrent(_selectedSerie);
            return _selectedSerie;
        }

        public async Task<DataResult<List<IShow>>> GetRecommendations()
        {
            //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
            var currentUser = _userService.GetCurrentUser();
            var res = await _showDataService.GetRecommendationsByUser(UserTokenDtoFactory.GetDto(currentUser));
            if (res == null) return new DataResult<List<IShow>>(StandardResults.Error);
            return new DataResult<List<IShow>>(res.ToList());
        }

        public async Task<DataResult<List<IMiniShow>>> SearchShowsByKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return new DataResult<List<IMiniShow>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
            var res = await _showDataService.SearchShowsByKey(key);
            if (res == null)
            {
                var trending = await GetTrending();
                if (trending.IsOk && trending.Data != null && trending.Data.Count > 0)
                {
                    var list = trending.Data.Where(x => x.Title.ToLower().Contains(key.ToLower())).ToList();
                    if (list.Any()) return new DataResult<List<IMiniShow>>(list);
                }
                return new DataResult<List<IMiniShow>>(StandardResults.Error);
            }
            return new DataResult<List<IMiniShow>>(res);
        }

        public async void UpdateCurrentShow()
        {
            await SetCurrent(_selectedSerie);
        }


        public async Task<DataResult<IRateResult>> RateShow(int rate)
        {
            //if (!await IsInternet()) return new DataResult<IRateResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var currentShow = GetCurrentShow();
            if (user == null || currentShow == null) return new DataResult<IRateResult>(StandardResults.Error);
            var res = await _showDataService.RateShow(UserTokenDtoFactory.GetDto(user), rate, ShowDtoFactory.GetDto(currentShow));
            if (res == null || !res.Status)
                return new DataResult<IRateResult>(StandardResults.Error);
            currentShow.UserRating = rate;  
            UpdateTrending();
            return new DataResult<IRateResult>(res);
        }

        public async Task<DataResult<IGenericPostResult>> AddToWatchlist()
        {
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var currentShow = GetCurrentShow();
            if (user == null || currentShow == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var res = await _showDataService.AddShowToWatchlist(UserTokenDtoFactory.GetDto(user), currentShow.Ids.TvDbId.Value, currentShow.Title, currentShow.Year.Value);
            if (res == null || res.Status == RequestResults.Failure)
                return new DataResult<IGenericPostResult>(StandardResults.Error);
            await SetCurrent(currentShow);
            UpdateTrending();
            UpdateWatchlist();
            //SetSomeEpisodeAsSeen();
            return new DataResult<IGenericPostResult>(res);
        }

        private async void SetSomeEpisodeAsSeen()
        {
            var currentShow = GetCurrentShow();
            if (currentShow == null) return;
            //if (currentShow.Seasons.Any(x => x.Number == 0))
            //{
            //    await CoreServices.Season.SetSeasonAsSeen(0);
            //}
            //else if (currentShow.Seasons.Any(x => x.Number == 1))
            //{
            //    await CoreServices.Episode.SetAsSeen(1, 1);
            //}
        }

        public async Task<DataResult<IGenericPostResult>> RemoveFromWatchlist()
        {
            //if (!await IsInternet()) return new DataResult<IGenericPostResult>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            var currentShow = GetCurrentShow();
            if (user == null || currentShow == null) return new DataResult<IGenericPostResult>(StandardResults.Error);
            var res = await _showDataService.RemoveShowFromWatchlist(UserTokenDtoFactory.GetDto(user), currentShow.Ids.TvDbId.Value, currentShow.Title, currentShow.Year.Value);
            if (res == null || res.Status == RequestResults.Failure)
                return new DataResult<IGenericPostResult>(StandardResults.Error);
            await SetCurrent(currentShow);
            UpdateTrending();
            UpdateWatchlist();
            return new DataResult<IGenericPostResult>(res);
        }


        //public async Task<DataResult<List<IShow>>> GetByCategory(ICategory category)
        //{
        //    if (category == null) return new DataResult<List<IShow>>(StandardResults.Error);
        //    //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
        //    var res = await GetTrending();
        //    if (res == null || !res.IsOk || res.Data == null)
        //        return new DataResult<List<IShow>>(StandardResults.Error);
        //    var x = res.Data.Where(a => a.Genres.Contains(category.Name)).ToList();
        //    return new DataResult<List<IShow>>(x);
        //}

        public async Task<DataResult<List<IShow>>> GetShowsWatchlistByUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<List<IShow>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
            //  var res = await _showDataService.GetShowsWatchlistByUser(username);
            List<IShow> res = null;
            if (res == null) return new DataResult<List<IShow>>(StandardResults.Error);
            return new DataResult<List<IShow>>(res);
        }

        public async Task<DataResult<List<IShow>>> GetWatchlistMyUser()
        {
            //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
            var user = _userService.GetCurrentUser();
            if (user == null) return new DataResult<List<IShow>>(StandardResults.Error);
            var res = await _showDataService.GetShowsWatchlistByUser(UserTokenDtoFactory.GetDto(user), true);
            return res == null ? new DataResult<List<IShow>>(StandardResults.Error) : new DataResult<List<IShow>>(res);
        }

        public async void UpdateWatchlist()
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return;
            var res = await _showDataService.GetShowsWatchlistByUser(UserTokenDtoFactory.GetDto(user), true);
            UpdateProgress();
        }




        public async Task<DataResult<List<IMiniShow>>> GetAnime()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _showDataService.GetAnimeList(UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<List<IMiniShow>>(StandardResults.Error) : new DataResult<List<IMiniShow>>(res.OrderByDescending(x => x.Rating).ToList());
        }

        public async Task<DataResult<List<IMiniShow>>> GetTopImdb()
        {
            var currentUser = _userService.GetCurrentUser();
            var res = await _showDataService.GetTopImdb(UserTokenDtoFactory.GetDto(currentUser));
            return res == null ? new DataResult<List<IMiniShow>>(StandardResults.Error) : new DataResult<List<IMiniShow>>(res);
        }

        public void ClearTrending()
        {
            _trending = null;
        }



        public async Task<DataResult<List<IShow>>> GetShowsWithEpisodesWatchlistByUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return new DataResult<List<IShow>>(StandardResults.Error);
            //if (!await IsInternet()) return new DataResult<List<IShow>>(StandardResults.Offline);
            var res = await _showDataService.GetShowsWithEpisodesWatchlistByUser(username);
            if (res == null) return new DataResult<List<IShow>>(StandardResults.Error);
            return new DataResult<List<IShow>>(res);
        }



        //public async Task<DataResult<List<IShowProgress>>> GetShowProgress(string imdbId)
        //{
        //    //return new DataResult<List<IShowProgress>>(StandardResults.Error);
        //    var user = _userService.GetCurrentUser();
        //    if (user == null) return new DataResult<List<IShowProgress>>(StandardResults.Error);

        //    var res = await _showDataService.GetShowProgressAzure(imdbId, user.Username);
        //    if (res == null)
        //    {
        //        res = await _showDataService.GetShowProgress(imdbId, user.Username);
        //        if (res == null)
        //        {
        //            res = new List<IShowProgress>();
        //        }
        //    }
        //    var watchlist = await GetWatchlistMyUser();
        //    if (!watchlist.IsOk) return new DataResult<List<IShowProgress>>(res);
        //    foreach (var show in watchlist.Data)
        //    {
        //        if (res.Any(x => x.Show.Ids.ImdbId == show.Ids.ImdbId)) continue;
        //        var showComplete = await GetByImdbIdAzure(show.Ids.ImdbId);
        //        if (!showComplete.IsOk || showComplete.Data == null) showComplete = await GetByImdbId(show.Ids.ImdbId);
        //        if (showComplete.IsOk && showComplete.Data != null)
        //        {
        //            var newProgress = ShowProgressDtoFactory.CreateEmptyForShow(showComplete.Data);
        //            res.Add(newProgress);
        //        }
        //    }

        //    return new DataResult<List<IShowProgress>>(res);
        //}

        public async void UpdateProgress()
        {
            var user = _userService.GetCurrentUser();
            if (user == null) return;
           // var res = await _showDataService.GetShowProgress("", UserTokenDtoFactory.GetDto(user));
        }

        public async Task<DataResult<List<IShowProgress>>> GetShowProgress()
        {
            var user = _userService.GetCurrentUser();
            var res = await _showDataService.GetShowProgress(UserTokenDtoFactory.GetDto(user));
            if (res == null)
            {
                return new DataResult<List<IShowProgress>>(StandardResults.Error);
            }
            return new DataResult<List<IShowProgress>>(res);
        }

        public async Task<DataResult<List<IShow>>> GetLovedByUser(string username)
        {
            var res = await _showDataService.GetLovedByUser(username);
            if (res == null)
            {
                return new DataResult<List<IShow>>(StandardResults.Error);
            }
            return new DataResult<List<IShow>>(res);
        }

        public async Task<DataResult<double?>> GetImdbRanting(string imdbId)
        {
            //if (!await IsInternet()) return new DataResult<double?>(StandardResults.Offline);
            var res = await _showDataService.GetImdbRanting(imdbId);
            if (res == null) return new DataResult<double?>(StandardResults.Error);
            return new DataResult<double?>(res);
        }
    }
}
