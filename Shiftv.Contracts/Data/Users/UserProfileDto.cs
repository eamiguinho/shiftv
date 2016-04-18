using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Comments;

namespace Shiftv.Contracts.Data.Users
{
    public class UserSettingsDto
    {
        [JsonProperty(PropertyName = "user")]
        public UserDto User { get; set; }

        [JsonProperty(PropertyName = "account")]
        public AccountDto Account { get; set; }

        [JsonProperty(PropertyName = "connections")]
        public ConnectionsDto Connections { get; set; }

        [JsonProperty(PropertyName = "sharing_text")]
        public SharingTextDto SharingText { get; set; }
    }

    public class AccountDto
    {
        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }

        [JsonProperty(PropertyName = "CoverImage")]
        public string CoverImage { get; set; }
    }

    public class ConnectionsDto
    {
        [JsonProperty(PropertyName = "facebook")]
        public bool Facebook { get; set; }

        [JsonProperty(PropertyName = "twitter")]
        public bool Twitter { get; set; }

        [JsonProperty(PropertyName = "google")]
        public bool Google { get; set; }

        [JsonProperty(PropertyName = "tumblr")]
        public bool Tumblr { get; set; }
    }

    public class SharingTextDto
    {
        [JsonProperty(PropertyName = "watching")]
        public string Watching { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public string Watched { get; set; }
    }

    public class UserProfileDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "age")]
        public int? Age { get; set; }
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }
        [JsonProperty(PropertyName = "about")]
        public string About { get; set; }
        [JsonProperty(PropertyName = "joined")]
        public long? Joined { get; set; }
        [JsonProperty(PropertyName = "last_login")]
        public long? LastLogin { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "vip")]
        public bool IsVip { get; set; }

        [JsonProperty(PropertyName = "plays")]
        public int Plays { get; set; }

        [JsonProperty(PropertyName = "protected")]
        public bool Protected { get; set; }

        [JsonProperty(PropertyName = "stats")]
        public UserProfileStatsDto Stats { get; set; }

        //[JsonProperty(PropertyName = "watching")]
        //public List<UserProfileWatchingDto> Watching { get; set; }

        [JsonProperty(PropertyName = "watched")]
        public List<UserProfileWatchedDto> Watched { get; set; }

    }
}