using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Networks;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Networks
{
    public class NetworkTraktDataService : INetworkTraktDataService
    {
        private INetworkTraktQueryService _queryService;

        public NetworkTraktDataService(INetworkTraktQueryService queryService)
        {
            _queryService = queryService;
        }

        public Task<List<IUser>> GetFollowRequests(UserTokenDto userAccount)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetFollowRequests();
                    var res = await TraktDataServiceHelper.GetObjectWithCredentials<List<UserDto>>(url, userAccount);
                    return res.Select(UserDtoFactory.Create).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<INetworkApproveDenyResult> ApproveFollowerRequest(UserTokenDto userAccount, string username, bool followBack)
        {
            return Task.Run(async () => 
            {
                try
                {
                    var url = await _queryService.ApproveFollowerRequest();
                    var approveReq = new NetworkApproveDenyRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        User = username,
                        FollowBack = followBack
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(approveReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<NetworkApproveDenyResultDto>(url, myContent);
                    return NetworkApproveDenyResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<INetworkApproveDenyResult> DenyFollowerRequest(UserTokenDto userAccount, string username)
        {
            return Task.Run(async () => 
            {
                try
                {
                    var url = await _queryService.DenyFollowerRequest();
                    var approveReq = new NetworkApproveDenyRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        User = username
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(approveReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<NetworkApproveDenyResultDto>(url, myContent);
                    return NetworkApproveDenyResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<INetworkFollowResult> Follow(UserTokenDto userAccount, string username)
        {
            return Task.Run(async () => 
            {
                try
                {
                    var url = await _queryService.Follow();
                    var approveReq = new NetworkFollowRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        User = username
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(approveReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<NetworkFollowResultDto>(url, myContent);
                    return NetworkFollowResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<INetworkFollowResult> Unfollow(UserTokenDto userAccount, string username)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.Unfollow();
                    var approveReq = new NetworkFollowRequestJsonDto()
                    {
                        //Username = userAccount.Username.Trim(),
                        //Password = userAccount.PasswordEnc,
                        User = username
                    };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(approveReq));
                    var x = await TraktDataServiceHelper.PostWithCredentials<NetworkFollowResultDto>(url, myContent);
                    return NetworkFollowResultDtoFactory.Create(x);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        } 
    }
}
