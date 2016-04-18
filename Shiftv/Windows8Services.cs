using Autofac;
using Shiftv.Contracts.PlatformSpecificServices;
using Shiftv.Contracts.Services.PlatformServices;
using Shiftv.Global;
using Shiftv.PlatformServices;

namespace Shiftv
{
    public class Windows8Services
    {
        public static void RegisterTypes()
        {
            Ioc.Instance.RegisterType<DownloadService>().As<IDownloadService>().SingleInstance();
            Ioc.Instance.RegisterType<DataBackupService>().As<IDataBackupService>().SingleInstance();
        }
    }
}