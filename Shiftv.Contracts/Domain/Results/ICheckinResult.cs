namespace Shiftv.Contracts.Domain.Results
{
    public interface ICheckinResult
    {
        RequestResults Status { get; set; }

        ICheckInTimestampsResult TimeStamps { get; set; }

        ICheckInShowResult Show { get; set; }

        string Message { get; set; }

        bool Facebook { get; set; }

        bool Twitter { get; set; }

        bool Tumblr { get; set; }

        bool Path { get; set; }

        string Error { get; set; }
        int Wait { get; set; }
    }
    public interface ICheckInTimestampsResult
    {
        long Start { get; set; }
        long End { get; set; }
        long ActiveFor { get; set; }
    }

    public interface ICheckInShowResult
    {
        string Title { get; set; }
        int Year { get; set; }
        string ImdbId { get; set; }
        int TvdbId { get; set; }
    }
}