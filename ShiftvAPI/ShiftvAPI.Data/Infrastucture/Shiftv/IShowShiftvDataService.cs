using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Data.Sync;

namespace ShiftvAPI.Contracts.Infrastucture.Shiftv
{
    public interface IShowShiftvDataService
    {
        Task<Show> GetShowById(int id);
        void SaveShow(Show showData);
        void SaveSeason(int imdbId, List<Season> season);
        Task<List<MiniShow>> GetTrending(int page, int limit);
        Task SaveTrending(List<MiniShow> listMini);
        Task<List<MiniShow>> GetPopular(int page, int limit);
        Task SavePopular(List<MiniShow> listMini);
        DateTime? GetLastUpdate();
        void SaveLastUpdate(DateTime now);
        Task<People> GetPeople(int showId);
        Task SavePeople(People peopleData, int showId);
        Task<List<MiniShow>> Search(string key);
        List<Season> GetSeasons(int id);
        Task<List<Comment>> GetComments(int showId);
        Season GetLastSeason(int showId);
        Task<List<MiniShow>> GetUserShows(string token, List<WatchedEpisodes> userShowsIds);
    }
}
