using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;

namespace Shiftv.Contracts.Services.PlatformServices
{
    public interface IDownloadService
    {
        void DoDownload(Uri uri, IEpisode episode);
        void ResumeDownloads();
        Task<List<EpisodeDto>> GetDownloadedEpisodes();
        Task<Dictionary<Guid, EpisodeDto>> GetDownloadingEpisodes();

        void PauseDownload(Guid downloadId);
        void ResumeDownload(Guid downloadId);
        void CancelDownload(Guid downloadId);

        Task DeleteDownloadedEpisode(IEpisode episode);
    }

}
