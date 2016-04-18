using Newtonsoft.Json;
using Shiftv.Contracts.Domain.Images;

namespace Shiftv.Core.Models.Images
{
    public class Image : IImage
    {
        //public string Poster { get; set; }
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
        //public string FanartSmall
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Fanart)) return null;
        //        var split = Fanart.Remove(Fanart.Length - 4);
        //        split += "-218.jpg";
        //        return split;
        //    }
        //}

        //public string Banner { get; set; }
        //public string Screen { get; set; }
        //public string ScreenSmall
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(Screen)) return null;
        //        var split = Screen.Remove(Screen.Length - 4);
        //        split += "-218.jpg";
        //        return split;
        //    }
        //}
        public IFanart Fanart { get; set; }
        public IPoster Poster { get; set; }
        public ILogo Logo { get; set; }
        public IClearart Clearart { get; set; }
        public IBanner Banner { get; set; }
        public IThumb Thumb { get; set; }
        public IHeadshot Headshot { get; set; }
        public IAvatar Avatar { get; set; }
        public IScreenshot Screenshot { get; set; }
    }
}