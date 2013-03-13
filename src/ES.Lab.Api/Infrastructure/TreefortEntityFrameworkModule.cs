using System.Data.Entity;
using Autofac;
using Autofac.Integration.WebApi;
using ES.Lab.Infrastructure.Data;
using Treefort.EntityFramework.Eventing;
using Treefort.Events;

namespace ES.Lab.Api.Infrastructure
{
    public class TreefortEntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof (IEventContext)))
                .Except<EventContext>()
                .Except<EventStore>()
                .AsImplementedInterfaces();
            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EventContext, EventConfiguration>());
            //TODO DbConfiguration.SetConfiguration for multitenant
            builder.RegisterType<EventContext>().AsImplementedInterfaces().InstancePerApiRequest();

            builder.RegisterType<EventStore>()
                .Named<IEventStore>("implementor");
        }
    }
}