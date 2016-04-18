using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Seasons;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Seasons
{
    public class SeasonTraktDataService : ISeasonTraktDataService
    {
         private ISeasonTraktQueryService _queryService;

        public SeasonTraktDataService(ISeasonTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<IGenericPostResult> SetSeasonAsSeen(UserTokenDto userAccount, int tvDbId, string imdbId, string title, int year, int season)
        {
            return Task.Run(async () =>
            {   
                try
                {
                    if (userAccount == null || tvDbId <= -1 || season <= -1 || year <= -1 || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId)) return null;
                    var url = await _queryService.SetSeasonAsSeen();
                    var req = new SetSeasonAsSeenRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        Title = title,
                        TvDbId = tvDbId,
                        Year = year,
                        ImdbId = imdbId,
                        Season = season
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(req));
                    var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
                    return GenericPostResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        //public Task<IGenericPostResult> SetSeasonAsUnSeen(UserAccountDto userAccount, int tvDbId, string imdbId, string title, int year, int season)
        //{
        //    return Task.Run(async () =>
        //    {
        //        try
        //        {
        //            if (userAccount == null || tvDbId <= -1 || season <= -1 || year <= -1 || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(imdbId)) return null;
        //            var url = await _queryService.SetSeasonAsUnSeen();
        //            var req = new SetSeasonAsSeenRequestJsonDto()
        //            {
        //                Username = userAccount.Username.Trim(),
        //                Password = userAccount.PasswordEnc,
        //                Title = title,
        //                TvDbId = tvDbId,
        //                Year = year,
        //                ImdbId = imdbId,
        //                Season = season
        //            };
        //            HttpContent myContent = new StringContent(JsonConvert.SerializeObject(req));
        //            var x = await TraktDataServiceHelper.PostWithCredentials<GenericPostResultDto>(url, myContent);
        //            return GenericPostResultDtoFactory.Create(x);
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    });
        //}
    }
}
