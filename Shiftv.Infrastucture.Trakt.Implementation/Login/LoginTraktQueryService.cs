using System.Threading.Tasks;
using Shiftv.Contracts.DataServices.Login;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Login
{
    public class LoginTraktQueryService : ILoginTraktQueryService
    {
        public Task<string> GetUserQuery()
        {
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}", 
                TraktConstants.BaseApiUrl, 
                TraktConstants.AccountResource,
                TraktConstants.SettingsAction, 
                TraktConstants.TraktKey));
        }

        public Task<string> GetLoginTest()
        {
            //"http://api.trakt.tv/account/test/" + TraktConstants.TraktKey;
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}", 
                TraktConstants.BaseApiUrl, 
                TraktConstants.AccountResource,
                TraktConstants.TestAction, 
                TraktConstants.TraktKey));
        }

        public Task<string> GetCreateAccount()
        {
            //http://api.trakt.tv/account/create/" + TraktConstants.TraktDevKey
            return Task.Run(() => string.Format("{0}/{1}/{2}/{3}", 
               TraktConstants.BaseApiUrl, 
               TraktConstants.AccountResource,
               TraktConstants.CreateAction, 
               TraktConstants.TraktDevKey));
        }
    }
}
