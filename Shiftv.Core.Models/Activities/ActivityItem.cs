using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Activities
{
    public class ActivityItem : IActivityItem
    {
        public int Timestamp { get; set; }
        public IActivityWhen When { get; set; }
        public IActivityElapsed Elapsed { get; set; }
        public ActivityTypes Type { get; set; }
        public ActivityActions Action { get; set; }
        public IUser User { get; set; }
        // public IUserProfile User { get; set; }
        public IEpisode Episode { get; set; }
        public IShow Show { get; set; }
        public int Id { get; set; }
        public IMovie Movie { get; set; }
        public string Rating { get; set; }
        public int? RatingAdvanced { get; set; }
        public bool? IsRatingAdvanced { get; set; }
    }
}