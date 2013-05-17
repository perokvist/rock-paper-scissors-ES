using Autofac;
using Treefort.WebApi.Security;

namespace ES.Lab.Api.Infrastructure
{
    public class TreefortWebApiModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .Where(t => !t.FullName.Contains(".HelpPage"))
             .Except<GenericRoleProvider>()
             .Except<GenericAuthenticationService>()
             .AsImplementedInterfaces();

            builder.RegisterType<BasicAuthenticationMessageHandler>()
                .AsSelf();

            builder.RegisterType<PrincipalProvider>()
                   .AsImplementedInterfaces();

            builder.Register(c => new GenericAuthenticationService((u, p) => (u.EndsWith("jayway.com") && p == "eslab")))
                .AsImplementedInterfaces();

            builder.Register(c => new GenericRoleProvider(x => new[] { "Player" }))
                .AsImplementedInterfaces();
        }
    }
}