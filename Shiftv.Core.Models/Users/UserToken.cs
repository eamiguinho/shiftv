using System;
using Shiftv.Contracts.Domain.Users;

namespace Shiftv.Core.Models.Users
{
    public class UserToken : IUserToken
    {
        public IUserSettings UserSettings { get; set; }
        public string AccessToken { get; set; }

        public string TraktAccessToken { get; set; }

        public string TokenType { get; set; }
        public int? ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}