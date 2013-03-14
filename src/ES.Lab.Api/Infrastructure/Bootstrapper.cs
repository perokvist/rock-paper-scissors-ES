using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using Autofac;
using Autofac.Integration.WebApi;
using ES.Lab.Domain;
using ES.Lab.Infrastructure.Data;
using ES.Lab.Api.Infrastructure.Security;
using Microsoft.AspNet.SignalR;
using Treefort;
using Treefort.Commanding;
using Treefort.EntityFramework.Eventing;
using Treefort.Events;
using Treefort.Infrastructure;
using Treefort.Read;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace ES.Lab.Api.Infrastructure
{
    public class Bootstrapper
    {
        public static IDependencyResolver Start()
        {
            var cb = new ContainerBuilder();

            cb.RegisterModule<InfrastructureModule>();
            cb.RegisterModule<TreefortEntityFrameworkModule>();
            cb.RegisterModule<TreefortModule>();

            cb
                .RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .Except<GenericRoleProvider>()
                .Except<GenericAuthenticationService>()
                .AsImplementedInterfaces();

            //Route commands
            cb.Register(c => new CommandRouteConfiguration()
                                      .Tap(config => ((ICommandRouteConfiguration)config).Add<ICommand, Game>()))
                                      .AsImplementedInterfaces()
                                      .SingleInstance();

            RegisterSecurity(cb);

            //Web Api
            cb.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());

            //SignalR
            cb.RegisterInstance(GlobalHost.ConnectionManager).AsImplementedInterfaces().ExternallyOwned();

            return new AutofacWebApiDependencyResolver(cb.Build());
        }


        private static void RegisterSecurity(ContainerBuilder cb)
        {
            cb.RegisterType<BasicAuthenticationMessageHandler>()
                .AsSelf();

            cb.Register(c => new GenericAuthenticationService((u, p) => (u.EndsWith("jayway.com") && p == "eslab")))
                .AsImplementedInterfaces();

            cb.Register(c => new GenericRoleProvider(x => new[] {"Player"}))
                .AsImplementedInterfaces();
        }
    }
}