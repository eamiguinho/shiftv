using Newtonsoft.Json;

namespace Shiftv.Contracts.Data.Images
{
    public class ImageDto
    {
        //[JsonProperty(PropertyName = "poster")]
        //public string Poster { get; set; }
        //[JsonProperty(PropertyName = "fanart")]
        //public string Fanart { get; set; }

        //public string FanartReduced
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Fanart)) return null;
        //        var split = Fanart.Remove(Fanart.Length - 4);
        //        split += "-940.jpg";
        //        return split;
        //    }
        //}

        //[JsonProperty(PropertyName = "banner")]
        //public string Banner { get; set; }
        // [JsonProperty(PropertyName = "screen")]
        //public string Screen { get; set; }

        [JsonProperty(PropertyName = "fanart")]
        public FanartDto Fanart { get; set; }

        [JsonProperty(PropertyName = "poster")]
        public PosterDto Poster { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public LogoDto Logo { get; set; }

        [JsonProperty(PropertyName = "clearart")]
        public ClearartDto Clearart { get; set; }

        [JsonProperty(PropertyName = "banner")]
        public BannerDto Banner { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public ThumbDto Thumb { get; set; }

        [JsonProperty(PropertyName = "headshot")]
        public HeadshotDto Headshot { get; set; }

        [JsonProperty(PropertyName = "avatar")]
        public AvatarDto Avatar { get; set; }

        [JsonProperty(PropertyName = "screenshot")]
        public ScreenshotDto Screenshot { get; set; }
    }

    public class FanartDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class PosterDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class HeadshotDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    public class ScreenshotDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }

        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }


    public class LogoDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class ClearartDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class BannerDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class ThumbDto
    {
        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }

    public class AvatarDto
    {

        [JsonProperty(PropertyName = "full")]
        public string Full { get; set; }
    }
}