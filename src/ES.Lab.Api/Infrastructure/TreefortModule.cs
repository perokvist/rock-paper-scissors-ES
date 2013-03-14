using System.Collections.Generic;
using Autofac;
using Treefort;
using Treefort.Events;
using Treefort.Infrastructure;
using Treefort.Read;

namespace ES.Lab.Api.Infrastructure
{
    public class TreefortModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(System.Reflection.Assembly.GetAssembly(typeof (IApplicationService)))
                .Except<InMemoryEventStore>()
                .Except<InMemoryEventStream>()
                .Except<DelegatingEventStore>()
                .Except<EventListener>()
                .Except<CommandRouteConfiguration>()
                .AsImplementedInterfaces();

            //cb.RegisterType<InMemoryEventStore>()
            // .Named<IEventStore>("implementor")
            // .SingleInstance();

            builder.RegisterDecorator<IEventStore>(
             (c, inner) => new DelegatingEventStore(inner, c.Resolve<IEnumerable<IEventListener>>()),
             fromKey: "implementor");
         

         
        }
    }
}