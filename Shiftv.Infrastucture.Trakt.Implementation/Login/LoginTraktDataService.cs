using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Data.Results;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.DataServices.Login;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Infrastucture.Trakt.Implementation.Helpers;

namespace Shiftv.Infrastucture.Trakt.Implementation.Login
{
    public class LoginTraktDataService : ILoginTraktDataService
    {
        private readonly ILoginTraktQueryService _queryService;
        private IDataBackupService _backupService;

        public LoginTraktDataService(ILoginTraktQueryService queryService, IDataBackupService backupService)
        {
            _queryService = queryService;
            _backupService = backupService;
        }
        public Task<LoginUserResult> LoginToTrakt(string username, string password)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var url = await _queryService.GetLoginTest();
                    var loginReq = new LoginRequestJsonDto { Username = username.Trim(), Password = password };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(loginReq));
                    var httpClient = TraktDataServiceHelper.GetHttpClient();
                    httpClient.Timeout = new TimeSpan(0,0,1,0);
                    var responseBodyAsText = await httpClient.PostAsync(url, myContent);
                    if (responseBodyAsText.IsSuccessStatusCode)
                    {
                        var res = await responseBodyAsText.Content.ReadAsStringAsync();
                        var x = JsonConvert.DeserializeObject<LoginResponseJsonDto>(res);
                        return x.Status != "success" ? LoginUserResult.Error(x.Message) : LoginUserResult.Ok();
                    }
                    else
                    {
                        if (responseBodyAsText.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            return LoginUserResult.Unauthorized();
                        }
                        return LoginUserResult.Offline();
                    }
                }
                catch (Exception e)
                {
                    return LoginUserResult.Error(e.Message);
                }
            });
        }

 
        public Task<CreateUserResult> CreateAccount(string username, string password, string email)
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
                        return CreateUserResult.Error();
                    var url2 = await _queryService.GetCreateAccount();
                    if (string.IsNullOrEmpty(url2)) return CreateUserResult.Error();
                    var loginReq = new CreateUserRequestJsonDto { Username = username.Trim(), Password = password, Email = email };
                    HttpContent myContent = new StringContent(JsonConvert.SerializeObject(loginReq));
                    var httpClient = new HttpClient();
                    var responseBodyAsText = await httpClient.PostAsync(url2, myContent);
                    var res = await responseBodyAsText.Content.ReadAsStringAsync();
                    var objectReceived = JsonConvert.DeserializeObject<CreateUserResponseJsonDto>(res);
                    return objectReceived.Status != "success" ? CreateUserResult.Error(objectReceived.Error) : CreateUserResult.Ok();
                }
                catch (Exception)
                {
                    return CreateUserResult.Error();
                }
            });

        }
    }


}
