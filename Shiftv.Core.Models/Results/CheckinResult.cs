using Shiftv.Contracts.Domain.Results;

namespace Shiftv.Core.Models.Results
{
    class CheckinResult : ICheckinResult
    {
        public RequestResults Status { get; set; }
        public ICheckInTimestampsResult TimeStamps { get; set; }
        public ICheckInShowResult Show { get; set; }
        public string Message { get; set; }
        public bool Facebook { get; set; }
        public bool Twitter { get; set; }
        public bool Tumblr { get; set; }
        public bool Path { get; set; }
        public string Error { get; set; }
        public int Wait { get; set; }
    }

    class CheckInTimestampsResult : ICheckInTimestampsResult
    {
        public long Start { get; set; }
        public long End { get; set; }
        public long ActiveFor { get; set; }
    }

    class CheckInShowResult : ICheckInShowResult
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string ImdbId { get; set; }
        public int TvdbId { get; set; }
    }
}
