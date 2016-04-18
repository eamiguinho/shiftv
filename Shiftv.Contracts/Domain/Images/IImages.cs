using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;

namespace Shiftv.Contracts.Domain.Images
{
    public interface IImage
    {
        //string Poster { get; set; }
        //string Fanart { get; set; }

        //string FanartReduced { get; }
        //string FanartSmall { get; }

        //string Banner { get; set; }
        //string Screen { get; set; }

        //string ScreenSmall { get;  }
        IFanart Fanart { get; set; }

        IPoster Poster { get; set; }

        ILogo Logo { get; set; }

        IClearart Clearart { get; set; }

        IBanner Banner { get; set; }

        IThumb Thumb { get; set; }

        IHeadshot Headshot { get; set; }

        IAvatar Avatar { get; set; }

        IScreenshot Screenshot { get; set; }
    }
}