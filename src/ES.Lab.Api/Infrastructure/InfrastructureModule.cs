using System.Data.Entity;
using Autofac;
using Autofac.Integration.WebApi;
using ES.Lab.Infrastructure.Data;
using ES.Lab.Infrastructure.Signaling;
using Treefort.Events;

namespace ES.Lab.Api.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof (IProjectionContext)))
                .Except<InMemoryProjectionContext>()
                .Except<ProjectionContext>()
                .AsImplementedInterfaces();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectionContext, Lab.Infrastructure.ProjectionMigrations.Configuration>());
            //TODO DbConfiguration.SetConfiguration for multitenant
            builder.RegisterType<ProjectionContext>().AsImplementedInterfaces().InstancePerApiRequest();

        }
    }
}