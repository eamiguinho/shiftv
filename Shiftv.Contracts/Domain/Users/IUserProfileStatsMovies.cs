namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserProfileStatsMovies
    {
        int Watched { get; set; }
        int Scrobbles { get; set; }
        int Checkins { get; set; }
        int Seen { get; set; }
        int Shouts { get; set; }
        int Loved { get; set; }
        int Hated { get; set; }
    }
}