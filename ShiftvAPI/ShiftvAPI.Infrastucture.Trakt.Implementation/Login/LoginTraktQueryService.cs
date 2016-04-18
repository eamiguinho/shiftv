using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Login;
using ShiftvAPI.Infrastucture.Trakt.Implementation.Helpers.Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace ShiftvAPI.Infrastucture.Trakt.Implementation.Login
{
    class LoginTraktQueryService : ILoginTraktQueryService
    {
        public Task<string> GetToken()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
               TraktConstants.BaseApiUrl,
               TraktConstants.OAuth,
               TraktConstants.OAuthToken
               ));
        }

        public Task<TokenRequest> GetTokenPost(string code)
        {
            return Task.Run(() =>
            {
                var tokenReq = new TokenRequest();
                tokenReq.ClientId = TraktConstants.OAuthClientId;
                tokenReq.ClientSecret = TraktConstants.OAuthClientSecret;
                tokenReq.Code = code;
                tokenReq.GrantType = TraktConstants.OAuthGrantTypeAuth;
                tokenReq.RedirectUri = TraktConstants.OAuthRedirectUri;
                return tokenReq;
            });
        }

        public Task<string> GetUserSettings()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/",
             TraktConstants.BaseApiUrl,
             TraktConstants.UsersAction,
             TraktConstants.SettingsAction
             ));
        }
    }
}