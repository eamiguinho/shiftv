using Windows.ApplicationModel;
using Autofac;
using Shiftv.Contracts.Services.PlatformServices;
using Shiftv.Core.Models;
using Shiftv.Global;
using Shiftv.Infrastucture.Trakt.Implementation;
using Shiftv.Services.Implementation;

namespace Shiftv
{
    public class Bootstraper
    {
        public Bootstraper()
        {
            CoreModelsIoc.RegisterTypes();
           
           
            if (DesignMode.DesignModeEnabled)
            {
                CoreServicesIoc.RegisterDesignMode();
            }
            else
            { 
           
                CoreServicesIoc.Register();
                TraktServiceIoc.Register();
                 Windows8Services.RegisterTypes();
                var x = Ioc.Container.Resolve<IDownloadService>();
                x.ResumeDownloads();
            }
        }
    }
}
