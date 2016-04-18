namespace Shiftv.Contracts.Domain.Users
{
    public interface IUserProfileStatsShows 
    {
        int Library { get; set; }
        int Watched { get; set; }
        int Collection { get; set; }
        int Shouts { get; set; }
        int Loved { get; set; }
        int Hated { get; set; }
    }
}