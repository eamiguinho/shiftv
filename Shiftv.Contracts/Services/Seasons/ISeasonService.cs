using System.Threading.Tasks;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Global;

namespace Shiftv.Contracts.Services.Seasons
{
    public interface ISeasonService
    {
        Task<DataResult<IGenericPostResult>> SetSeasonAsSeen(int season);
    }
}