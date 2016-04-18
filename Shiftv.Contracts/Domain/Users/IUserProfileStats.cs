namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserProfileStats
    {
        int Friends { get; set; }
        IUserProfileStatsShows Shows { get; set; }
        IUserProfileStatsEpisodes Episodes { get; set; }
        IUserProfileStatsMovies Movies { get; set; }
    }
}