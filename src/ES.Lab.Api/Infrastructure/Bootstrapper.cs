using Autofac;
using Autofac.Integration.WebApi;
using ES.Lab.Domain;
using Microsoft.AspNet.SignalR;
using Treefort.Commanding;
using Treefort.Infrastructure;
using Treefort.WebApi.Security;

namespace ES.Lab.Api.Infrastructure
{
    public class Bootstrapper
    {
        public static System.Web.Http.Dependencies.IDependencyResolver Start()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<InfrastructureModule>();
            cb.RegisterModule<TreefortEntityFrameworkModule>();
            cb.RegisterModule<TreefortWebApiModule>();
            cb.RegisterModule<TreefortModule>();
            
            //Route commands
            cb.Register(c => new CommandRouteConfiguration()
                                      .Tap(config => ((ICommandRouteConfiguration)config).Add<ICommand, Game>()))
                                      .AsImplementedInterfaces()
                                      .SingleInstance();

            //Web Api
            cb.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());

            //SignalR
            cb.RegisterInstance(GlobalHost.ConnectionManager).AsImplementedInterfaces().ExternallyOwned();

            return new AutofacWebApiDependencyResolver(cb.Build());
        }
        
    }
}