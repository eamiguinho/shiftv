using System.Runtime.InteropServices;
using Autofac;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Domain.Images;
using Shiftv.Global;

namespace Shiftv.Contracts.Data.Factories
{
    public static class ImageDtoFactory
    {
        public static IImage Create(ImageDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IImage>();
            myImage.Banner = BannerDtoFactory.Create(dto.Banner);
            myImage.Fanart = FanartDtoFactory.Create(dto.Fanart);
            myImage.Poster = PosterDtoFactory.Create(dto.Poster);
            myImage.Logo = LogoDtoFactory.Create(dto.Logo);
            myImage.Screenshot = ScreenshotDtoFactory.Create(dto.Screenshot);
            myImage.Thumb = ThumbDtoFactory.Create(dto.Thumb);
            myImage.Avatar = AvatarDtoFactory.Create(dto.Avatar);
            myImage.Clearart = ClearartDtoFactory.Create(dto.Clearart);
            myImage.Headshot = HeadshotDtoFactory.Create(dto.Headshot);
            return myImage;
        }

        public static ImageDto GetDto(IImage dto)
        {
            if (dto == null) return null;
            var myImage = new ImageDto();
            myImage.Banner = BannerDtoFactory.GetDto(dto.Banner);
            myImage.Fanart = FanartDtoFactory.GetDto(dto.Fanart);
            myImage.Poster = PosterDtoFactory.GetDto(dto.Poster);
            myImage.Logo = LogoDtoFactory.GetDto(dto.Logo);
            myImage.Screenshot = ScreenshotDtoFactory.GetDto(dto.Screenshot);
            myImage.Thumb = ThumbDtoFactory.GetDto(dto.Thumb);
            myImage.Avatar = AvatarDtoFactory.GetDto(dto.Avatar);
            myImage.Clearart = ClearartDtoFactory.GetDto(dto.Clearart);
            myImage.Headshot = HeadshotDtoFactory.GetDto(dto.Headshot);
            return myImage;
        }
    }

    public class HeadshotDtoFactory
    {
        public static IHeadshot Create(HeadshotDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IHeadshot>();
            myImage.Full = dto.Full;
            myImage.Medium = dto.Medium;
            myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static HeadshotDto GetDto(IHeadshot dto)
        {
            if (dto == null) return null;
            var myImage = new HeadshotDto();
            myImage.Full = dto.Full;
            myImage.Medium = dto.Medium;
            myImage.Thumb = dto.Thumb;
            return myImage;
        }
    }

    public class ClearartDtoFactory
    {
        public static IClearart Create(ClearartDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IClearart>();
            myImage.Full = dto.Full;
            //myImage.Medium = dto.Medium;
            //myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static ClearartDto GetDto(IClearart dto)
        {
            if (dto == null) return null;
            var myImage = new ClearartDto();
            myImage.Full = dto.Full;
            //myImage.Medium = dto.Medium;
            //myImage.Thumb = dto.Thumb;
            return myImage;
        }
    }

    public class AvatarDtoFactory   
    {
        public static IAvatar Create(AvatarDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IAvatar>();
            myImage.Full = dto.Full;
            //myImage.Medium = dto.Medium;
            //myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static AvatarDto GetDto(IAvatar dto)
        {
            if (dto == null) return null;
            var myImage = new AvatarDto();
            myImage.Full = dto.Full;
            //myImage.Medium = dto.Medium;
            //myImage.Thumb = dto.Thumb;
            return myImage;
        }
    }

    public class ThumbDtoFactory
    {
        public static IThumb Create(ThumbDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IThumb>();
            myImage.Full = dto.Full;
            return myImage;
        }

        public static ThumbDto GetDto(IThumb dto)
        {
            if (dto == null) return null;
            var myImage = new ThumbDto();
            myImage.Full = dto.Full;
            return myImage;
        }
    }

    public class ScreenshotDtoFactory   
    {
        public static IScreenshot Create(ScreenshotDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IScreenshot>();
            myImage.Full = dto.Full;
            myImage.Medium = dto.Medium;
            myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static ScreenshotDto GetDto(IScreenshot data)
        {
            if (data == null) return null;
            var myImage = new ScreenshotDto();
            myImage.Full = data.Full;
            myImage.Medium = data.Medium;
            myImage.Thumb = data.Thumb;
            return myImage;
        }
    }

    public class LogoDtoFactory
    {
        public static ILogo Create(LogoDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<ILogo>();
            myImage.Full = dto.Full;
            //myImage.Medium = dto.Medium;
            //myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static LogoDto GetDto(ILogo data)
        {
            if (data == null) return null;
            var myImage = new LogoDto();
            myImage.Full = data.Full;
            return myImage;
        }
    }

    public class PosterDtoFactory
    {
        public static IPoster Create(PosterDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IPoster>();
            myImage.Full = dto.Full;
            myImage.Medium = dto.Medium;
            myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static PosterDto GetDto(IPoster data)
        {
            if (data == null) return null;
            var myImage = new PosterDto();
            myImage.Full = data.Full;
            myImage.Medium = data.Medium;
            myImage.Thumb = data.Thumb;
            return myImage;
        }
    }

    public static class BannerDtoFactory
    {
        public static IBanner Create(BannerDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IBanner>();
            myImage.Full = dto.Full;
            return myImage;
        }

        public static BannerDto GetDto(IBanner banner)
        {
            if (banner == null) return null;
            var myImage = new BannerDto();
            myImage.Full = banner.Full;
            return myImage;
        }
    }

    public static class FanartDtoFactory
    {
        public static IFanart Create(FanartDto dto)
        {
            if (dto == null) return null;
            var myImage = Ioc.Container.Resolve<IFanart>();
            myImage.Full = dto.Full;
            myImage.Medium = dto.Medium;
            myImage.Thumb = dto.Thumb;
            return myImage;
        }

        public static FanartDto GetDto(IFanart data)
        {
            if (data == null) return null;
            var myImage = new FanartDto();
            myImage.Full = data.Full;
            myImage.Medium = data.Medium;
            myImage.Thumb = data.Thumb;
            return myImage;
        }
    }
}