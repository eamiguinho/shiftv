using Shiftv.Contracts.Domain.Movies;

namespace Shiftv.Core.Models.Global
{
    class Ids : IIds
    {
        public int? TraktId { get; set; }
        public string Slug { get; set; }
        public int? TvDbId { get; set; }
        public string ImdbId { get; set; }
        public int? TmDbId { get; set; }
        public int? TvRageId { get; set; }
    }
}