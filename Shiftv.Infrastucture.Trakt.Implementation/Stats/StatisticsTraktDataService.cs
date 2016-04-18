using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Data.Stats;
using Shiftv.Contracts.DataServices.Stats;
using Shiftv.Contracts.Domain.Stats;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Stats
{
    public class StatisticsTraktDataService : IStatisticsTraktDataService
    {
        private IStatisticsTraktQueryService _queryService;

        public StatisticsTraktDataService(IStatisticsTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<IStatistics> GetShowStats(int tvDbId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (tvDbId <= -1) return null;
                    var url = await _queryService.GetShowStats(tvDbId);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<StatsDto>(url);
                    return StatsDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<IStatistics> GetEpisodeStats(int tvDbId, int season, int number)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (tvDbId <= -1 || season <= -1 || number <= -1) return null;
                    var url = await _queryService.GetEpisodeStats(tvDbId, season, number);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<StatsDto>(url);
                    return StatsDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }



        public Task<IStatistics> GetMovieStats(string imdbId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(imdbId)) return null;
                    var url = await _queryService.GetMovieStats(imdbId);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<StatsDto>(url);
                    return StatsDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public async Task<bool> PingServer()
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.PingServer();
                    HttpClient client = new HttpClient();
                    var cancellationTokenSource = new CancellationTokenSource(15000); //timeout
                    using (var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Head
                    })
                    {
                        using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token))
                        {
                            return response.StatusCode == HttpStatusCode.OK;
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public Task<double?> GetImdbRanting(string imdbId)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(imdbId)) return null;
                    var url = await _queryService.GetImdbRating(imdbId);
                    var x = await TraktDataServiceHelper.GetObjectWithoutCredentials<ImdbRatingDto>(url);
                    return x == null ? (double?)null : x.ImdbRating;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}
