namespace ShiftvAPI.Contracts.Data
{
    public class UserRating
    {
        public int TraktId { get; set; }
        public int? Rating { get; set; }

        public int? Episode { get; set; }
        public int? Season { get; set; }
    }
}