using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using ShiftvAPI.Contracts.Helpers;
using ShiftvAPI.Contracts.Services.Movies;
using ShiftvAPI.Contracts.Services.Shows;
using ShiftvAPI.Infrastucture.Implementation;
using ShiftvAPI.Infrastucture.Trakt.Implementation;
using ShiftvAPI.Services.Implementation;
using Timer = System.Timers.Timer;

[assembly: OwinStartup(typeof(ShiftvAPI.Startup))]

namespace ShiftvAPI
{
    public partial class Startup
    {
        public Timer UpdateTimer;
        public void Configuration(IAppBuilder app)
        {
          
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Server=tcp:v06528hyiz.database.windows.net,1433;Database=shiftvstaging;User ID=amiguinho@v06528hyiz;Password=25713423_Ee;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");

        
            ServicesIoc.Register();
            ShiftvDataServicesIoc.Register();
            TraktDataServicesIoc.Register();
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseActivator(new ContainerJobActivator(Ioc.Container));
            var ioc = Ioc.Container.Resolve<IShowService>();
            var iocmovie = Ioc.Container.Resolve<IMovieService>();
            RecurringJob.AddOrUpdate(() =>ioc.UpdateData(), Cron.Daily);
            RecurringJob.AddOrUpdate(() => iocmovie.UpdateData(), Cron.Daily);

            var options = new DashboardOptions
            {
                AuthorizationFilters = new[] 
             {
           new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    // Require secure connection for dashboard
                   RequireSsl = false,
        SslRedirect = false,
                    // Case sensitive login checking
                    LoginCaseSensitive = true,
                    // Users
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = "admin",
                            // Password as plain text
                            PasswordClear = "buddy1234"
                        }
                    }
                })
        }
            };
            app.UseHangfireDashboard("/Admin/jobs", options);
            app.UseHangfireServer();

        }
    }



    public class ContainerJobActivator : JobActivator
    {
        private IContainer _container;

        public ContainerJobActivator(IContainer container)
        {
            _container = container;
        }

        public override object ActivateJob(Type type)
        {
            var typeInterface = type.GetInterface("I" + type.Name);
            return _container.Resolve(typeInterface);
        }
    }
}
