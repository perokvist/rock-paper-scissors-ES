using System.Collections.Generic;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http.Dependencies;
using ES.Lab.Domain;
using ES.Lab.Infrastructure.Data;
using ES.Lab.Api.Infrastructure.Security;
using Treefort;
using Treefort.Events;
using Treefort.Infrastructure;
using ES.Lab.Infrastructure.Migrations;
using Treefort.Read;

namespace ES.Lab.Api.Infrastructure
{
    public class Bootstrapper
    {
        public static IDependencyResolver Start()
        {
            var cb = new ContainerBuilder();
            RegisterCoreDependencies(cb);
            RegisterSecurity(cb);
            EventProjections(cb);
            //TODO see notes in AppService
            cb.RegisterType<ApplicationService<Game>>().AsImplementedInterfaces();
            cb.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());
            //Data context
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectionContext, Configuration>());
            cb.RegisterType<ProjectionContext>().AsImplementedInterfaces().InstancePerApiRequest();
            return new AutofacWebApiDependencyResolver(cb.Build());
        }

        private static void RegisterCoreDependencies(ContainerBuilder cb)
        {
            cb.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly(),
                                     System.Reflection.Assembly.GetAssembly(typeof (IApplicationService)),
                                     System.Reflection.Assembly.GetAssembly(typeof(IProjectionContext)))
                .Except<InMemoryEventStore>()
                .Except<DelegatingEventStore>()
                .Except<GenericAuthenticationService>()
                .Except<GenericRoleProvider>()
                .Except<InMemoryProjectionContext>()
                .Except<ProjectionContext>()
                .Except<EventListner>()
                .PreserveExistingDefaults()
                .AsImplementedInterfaces();
        }

        private static void EventProjections(ContainerBuilder cb)
        {
            cb.RegisterType<InMemoryEventStore>()
                .Named<IEventStore>("implementor")
                .SingleInstance();

            cb.RegisterDecorator<IEventStore>(
                (c, inner) => new DelegatingEventStore(inner, c.Resolve<IEnumerable<IEventListner>>()),
                fromKey: "implementor");
            
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