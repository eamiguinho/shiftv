using System;
using System.Threading.Tasks;
using ShiftvAPI.Contracts.Data;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Infrastucture.Shiftv;
using ShiftvAPI.Contracts.Infrastucture.Trakt.Login;
using ShiftvAPI.Contracts.Services.Login;

namespace ShiftvAPI.Services.Implementation.Login
{
    class LoginService : ILoginService
    {
        private ILoginShiftvDataService _loginShiftvDataService;
        private ILoginTraktDataService _loginTraktDataService;

        public LoginService(ILoginShiftvDataService loginShiftvDataService, ILoginTraktDataService loginTraktDataService)
        {
            _loginShiftvDataService = loginShiftvDataService;
            _loginTraktDataService = loginTraktDataService;
        }

        public async Task<DataResult<TokenResult>> GetToken(string code)
        {
            if (string.IsNullOrEmpty(code)) return new DataResult<TokenResult>(StandardResults.Error);
            var resTrakt = await _loginTraktDataService.GetToken(code);
            if (resTrakt != null && string.IsNullOrEmpty(resTrakt.Error) && string.IsNullOrEmpty(resTrakt.ErrorDescription))
            {
                var userTraktInfo = await _loginTraktDataService.GetUserSettings(resTrakt.AccessToken);
                if (userTraktInfo == null)
                {   
                    return new DataResult<TokenResult>(StandardResults.Error);
                }
                resTrakt.UserSettings = userTraktInfo;
                if (resTrakt.ExpiresIn != null) resTrakt.ExpiresAt = DateTime.Now.AddSeconds(resTrakt.ExpiresIn.Value);
                var shiftvToken = await _loginShiftvDataService.SaveUserSettings(resTrakt);
                resTrakt.TraktAccessToken = resTrakt.AccessToken;
                resTrakt.AccessToken = shiftvToken;
                return new DataResult<TokenResult>(resTrakt);
            }
            else
            {
                return new DataResult<TokenResult>(StandardResults.Error);
            }
        }
    }
}
