namespace Shiftv.Contracts.Domain.Movies
{
    public interface IIds
    {
        int? TraktId { get; set; }

        string Slug { get; set; }

        int? TvDbId { get; set; }

        string ImdbId { get; set; }

        int? TmDbId { get; set; }

        int? TvRageId { get; set; }
    }
}