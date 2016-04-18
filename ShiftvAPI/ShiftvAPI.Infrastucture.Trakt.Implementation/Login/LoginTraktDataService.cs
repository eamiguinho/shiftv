using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Login;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Login
{
    class LoginTraktDataService : ILoginTraktDataService
    {
        private ILoginTraktQueryService _loginTraktQueryService;

        public LoginTraktDataService(ILoginTraktQueryService loginTraktQueryService)
        {
            _loginTraktQueryService = loginTraktQueryService;
        }
        public Task<TokenResult> GetToken(string code)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _loginTraktQueryService.GetToken();
                    var postData = await _loginTraktQueryService.GetTokenPost(code);
                    TokenResult x = await TraktDataServiceHelper.PostObjectWithoutCredentials<TokenResult>(url, postData);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public Task<UserSettings> GetUserSettings(string accessToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _loginTraktQueryService.GetUserSettings();
                    UserSettings x = await TraktDataServiceHelper.GetObjectWithCredentials<UserSettings>(url, accessToken);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}