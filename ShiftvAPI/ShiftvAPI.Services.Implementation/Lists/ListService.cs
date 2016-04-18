using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Lists;
using ShiftvAPI.Contracts.Services.Lists;

namespace ShiftvAPI.Services.Implementation.Lists
{
    class ListService : IListService
    {
        private IListShiftvDataService _listShiftvDataService;
        private IListTraktDataService _listTraktDataService;

        public ListService(IListShiftvDataService listShiftvDataService, IListTraktDataService listTraktDataService)
        {
            _listShiftvDataService = listShiftvDataService;
            _listTraktDataService = listTraktDataService;
        }
        public async Task<DataResult<TraktList>> GetList(string username, string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(username)) return new DataResult<TraktList>(StandardResults.Error);
            var listInfo = await _listShiftvDataService.GetList(username, id);
            if (listInfo == null)
            {
                listInfo = await _listTraktDataService.GetListInfo(username, id);
                if (listInfo != null)
                {
                    var listItems = await _listTraktDataService.GetListItems(username, id);
                    var list = listItems.Select(listItem => new TraktListItemMini
                    {
                        Movie = listItem.Movie != null ? new MiniMovie
                        {
                            Runtime = listItem.Movie.Runtime,
                            Genres = listItem.Movie.Genres,
                            Ids = listItem.Movie.Ids,
                            Rating = listItem.Movie.Rating,
                            Title = listItem.Movie.Title,
                            Votes = listItem.Movie.Votes,
                            Fanart = listItem.Movie.Images.Fanart
                        } : null,
                        ListedAt = listItem.ListedAt,
                        Show = listItem.Show != null ? new MiniShow
                            {
                                Ids = listItem.Show.Ids,
                                Rating = listItem.Show.Rating,
                                Title = listItem.Show.Title,
                                Votes = listItem.Show.Votes,
                                Fanart = listItem.Show.Images.Fanart,
                                Network = listItem.Show.Network,
                                FirstAired = listItem.Show.FirstAired,
                                Genres = listItem.Show.Genres,
                                Status = listItem.Show.Status,
                                Year = listItem.Show.Year
                            } : null,
                        Type = listItem.Type
                    }).ToList();
                    listInfo.Items = list;
                    await _listShiftvDataService.SaveList(listInfo, username, id);
                }
            }
            if (listInfo == null)
            {
                return new DataResult<TraktList>(StandardResults.Error);
            }
            return new DataResult<TraktList>(listInfo);
        }
        public async Task<DataResult<List<MiniShow>>> GetListShow(string username, string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(username)) return new DataResult<List<MiniShow>>(StandardResults.Error);
            var listInfo = await _listShiftvDataService.GetList(username, id);
            if (listInfo != null)
            {
                var list = listInfo.Items.Where(x => x.Type == "show").Select(listItem => new MiniShow
                {
                    Ids = listItem.Show.Ids, Rating = listItem.Show.Rating, Title = listItem.Show.Title, Votes = listItem.Show.Votes, Fanart = listItem.Show.Fanart, Network = listItem.Show.Network,Year = listItem.Show.Year,FirstAired = listItem.Show.FirstAired, Genres = listItem.Show.Genres,Status = listItem.Show.Status
                }).ToList();
                return new DataResult<List<MiniShow>>(list);
            }
            listInfo = await _listTraktDataService.GetListInfo(username, id);
            if (listInfo != null)
            {
                var list = await DownloadListFromTraktSaveLocal(username, id, listInfo);
                var list2 = list.Where(x => x.Type == "show").Select(listItem => new MiniShow
                {
                    Ids = listItem.Show.Ids,
                    Rating = listItem.Show.Rating,
                    Title = listItem.Show.Title,
                    Votes = listItem.Show.Votes,
                    Fanart = listItem.Show.Fanart,
                    Network = listItem.Show.Network,
                    FirstAired = listItem.Show.FirstAired,
                    Year = listItem.Show.Year,
                    Genres = listItem.Show.Genres,
                    Status = listItem.Show.Status
                }).ToList();
                return new DataResult<List<MiniShow>>(list2);
            }
            return new DataResult<List<MiniShow>>(StandardResults.Error);
        }

        public async Task<DataResult<List<MiniMovie>>> GetListMovie(string username, string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(username)) return new DataResult<List<MiniMovie>>(StandardResults.Error);
            var listInfo = await _listShiftvDataService.GetList(username, id);
            if (listInfo != null)
            {
                var list = listInfo.Items.Where(x => x.Type == "movie").Select(listItem => new MiniMovie
                {
                    Ids = listItem.Movie.Ids,
                    Rating = listItem.Movie.Rating,
                    Title = listItem.Movie.Title,
                    Votes = listItem.Movie.Votes,
                    Fanart = listItem.Movie.Fanart,
                    Runtime = listItem.Movie.Runtime,
                    Genres = listItem.Movie.Genres,
                    Released = listItem.Movie.Released
                }).ToList();
                return new DataResult<List<MiniMovie>>(list);
            }
            listInfo = await _listTraktDataService.GetListInfo(username, id);
            if (listInfo != null)
            {
                var list = await DownloadListFromTraktSaveLocal(username, id, listInfo);
                var list2 = list.Where(x => x.Type == "show").Select(listItem => new MiniMovie
                {
                    Ids = listItem.Movie.Ids,
                    Rating = listItem.Movie.Rating,
                    Title = listItem.Movie.Title,
                    Votes = listItem.Movie.Votes,
                    Fanart = listItem.Movie.Fanart,
                    Runtime = listItem.Movie.Runtime,
                    Genres = listItem.Movie.Genres,
                }).ToList();
                return new DataResult<List<MiniMovie>>(list2);
            }
            return new DataResult<List<MiniMovie>>(StandardResults.Error);
        }

        private async Task<List<TraktListItemMini>> DownloadListFromTraktSaveLocal(string username, string id, TraktList listInfo)
        {
            var listItems = await _listTraktDataService.GetListItems(username, id);
            var list = listItems.Select(listItem => new TraktListItemMini
            {
                Movie = listItem.Movie != null
                    ? new MiniMovie
                    {
                        Runtime = listItem.Movie.Runtime,
                        Genres = listItem.Movie.Genres,
                        Ids = listItem.Movie.Ids,
                        Rating = listItem.Movie.Rating,
                        Title = listItem.Movie.Title,
                        Votes = listItem.Movie.Votes,
                        Fanart = listItem.Movie.Images.Fanart,
                        Released = listItem.Movie.Released
                    }
                    : null,
                ListedAt = listItem.ListedAt,
                Show = listItem.Show != null
                    ? new MiniShow
                    {
                        Ids = listItem.Show.Ids,
                        Rating = listItem.Show.Rating,
                        Title = listItem.Show.Title,
                        Votes = listItem.Show.Votes,
                        Fanart = listItem.Show.Images.Fanart,
                        Network = listItem.Show.Network
                        ,Year = listItem.Show.Year,FirstAired = listItem.Show.FirstAired, Genres = listItem.Show.Genres,Status = listItem.Show.Status
                    }
                    : null,
                Type = listItem.Type
            }).ToList();
            listInfo.Items = list;
            await _listShiftvDataService.SaveList(listInfo, username, id);
            return list;
        }
    }
}
