using System.Collections.Generic;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http.Dependencies;
using ES.Lab.Domain;
using ES.Lab.Infrastructure;
using ES.Lab.Read;

namespace ES.Lab.Api.Infrastructure
{
    public class Bootstrapper
    {
        public static IDependencyResolver Start()
        {
            var cb = new ContainerBuilder();
            cb.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly(), 
                System.Reflection.Assembly.GetAssembly(typeof(IApplicationService)))
                .Except<InMemoryEventStore>()
                .Except<DelegatingEventStore>()
                .Except<GameDetailsProjection>()
                .Except<OpenGamesProjection>()
                .PreserveExistingDefaults()
                .AsImplementedInterfaces();

            cb.RegisterType<InMemoryEventStore>()
                .Named<IEventStore>("implementor")
                .SingleInstance();

            cb.RegisterDecorator<IEventStore>(
                    (c, inner) => new DelegatingEventStore(inner, c.Resolve<IEnumerable<IEventListner>>()),
                    fromKey: "implementor");

            cb.RegisterType<GameDetailsProjection>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            cb.RegisterType<OpenGamesProjection>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            //TODO see notes in AppService
            cb.RegisterType<ApplicationService<Game>>().AsImplementedInterfaces();
            
            cb.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());
            //TODO filters
            //cb.RegisterWebApiModelBinders();
            //cb.RegisterWebApiModelBinderProvider();
            
            return new AutofacWebApiDependencyResolver(cb.Build());
        } 
    }
}