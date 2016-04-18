using System;
using Shiftv.Contracts.Domain.Images;

namespace Shiftv.Contracts.Domain.Users
{
    public interface IUser
    {
        string Username { get; set; }
        bool Private { get; set; }
        string Name { get; set; }
        bool Vip { get; set; }
        string JoinedAt { get; set; }
        string Location { get; set; }
        string About { get; set; }
        string Gender { get; set; }
        int? Age { get; set; }
        IImage Images { get; set; }
    }

    public interface IUserToken
    {
        IUserSettings UserSettings { get; set; }
        string AccessToken { get; set; }
        string TraktAccessToken { get; set; }
        string TokenType { get; set; }
        int? ExpiresIn { get; set; }
        string RefreshToken { get; set; }
        string Scope { get; set; }
        string Error { get; set; }
        string ErrorDescription { get; set; }
        DateTime ExpiresAt { get; set; }
    }
}
