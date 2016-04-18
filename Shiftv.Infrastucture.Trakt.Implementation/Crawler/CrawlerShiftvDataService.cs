using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Crawler;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Media;
using Shiftv.Contracts.DataServices.Crawler;
using Shiftv.Contracts.Domain.Crawler;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Contracts.Services.Crawler;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Crawler
{
    class CrawlerShiftvDataService : ICrawlerShiftvDataService
    {
        private ICrawlerShiftvQueryService _queryService;
        private IDataBackupService _backupService;

        public CrawlerShiftvDataService(ICrawlerShiftvQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }

        public Task<List<INameMap>> GetPossibleNames(string imdbId, string crawlerSource)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(imdbId) || string.IsNullOrEmpty(crawlerSource)) return null;
                    var url = await _queryService.GetPossibleNames(imdbId, crawlerSource);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<List<NameMapDto>>(url);
                    return x.Select(NameMapDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public void SaveAzureEmbbeds(List<string> toList, string link)
        {
            var uri = new Uri(link);
            TraktDataServiceHelper.SaveToAzure(toList, uri.Host + uri.AbsolutePath, BackupContainerTypes.StreamData, _backupService);
        }

        public async Task<List<string>> GetAzureEmbbeds(string link)
        {
            try
            {
                var uri = new Uri(link);
                var trendingShows = await _backupService.GetFileFromAzureWithTime(uri.Host + uri.AbsolutePath, BackupContainerTypes.StreamData, new TimeSpan(0, 3, 0));
                if (trendingShows == null) return null;
                var objectReceived = JsonConvert.DeserializeObject<List<string>>(trendingShows);
                return objectReceived;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SaveAzureEmbbeds2(List<ILinkInfo> links)
        {
            //var uri = new Uri(link);
            //TraktDataServiceHelper.SaveToAzure(toList, uri.Host + uri.AbsolutePath, BackupContainerTypes.StreamData, _backupService);
        }

        public void SaveAzureEmbbeds2(List<ILinkInfo> downloadlink, string imdbId, int season, int episode)
        {
            var list = downloadlink.Select(LinkInfoDtoFactory.GetDto).ToList();
            foreach (var linkInfoDto in list)
            {
                linkInfoDto.IsCached = true;
            }
            TraktDataServiceHelper.SaveDirectToAzure(list, imdbId + season + episode, BackupContainerTypes.StreamData, _backupService);
        }


        public async Task<List<ILinkInfo>> GetAzureEmbbeds2(string imdbId, int season, int episode, CrawlerType type)
        {
            try
            {
                var trendingShows = await _backupService.GetFileFromAzureWithTime(imdbId + season + episode, BackupContainerTypes.StreamData, type != CrawlerType.Movie ? new TimeSpan(6, 0, 0) : new TimeSpan(48, 0, 0));
                if (trendingShows == null) return null;
                var objectReceived = JsonConvert.DeserializeObject<List<LinkInfoDto>>(trendingShows);
                return objectReceived.Select(LinkInfoDtoFactory.Create).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
