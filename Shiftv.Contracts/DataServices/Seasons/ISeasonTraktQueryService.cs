using System.Threading.Tasks;

namespace Shiftv.Contracts.DataServices.Seasons
{
    public interface ISeasonTraktQueryService
    {
        Task<string> SetSeasonAsSeen();
        Task<string> SetSeasonAsUnSeen();
    }
}