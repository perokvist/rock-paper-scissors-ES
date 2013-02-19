using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Lab.Commands;
using ES.Lab.Domain;
using ES.Lab.Events;
using ES.Lab.Infrastructure;
using ES.Lab.Read;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;

namespace ES.Lab.IntegrationTests
{
    public class ApplicationServiceTests
    {
        [Test]
        public void AggregateServiceShouldStoreEvent()
        {
            //Arrange
            var store = A.Fake<IEventStore>();
            var appservice = new ApplicationService<Game>(store);
            //Act
            appservice.Handle(new CreateGameCommand(Guid.NewGuid(), Guid.NewGuid(), "test", 3));

            //Assert
            store.CallsTo(s => s.Store(Guid.NewGuid(), 0, null))
                .WhenArgumentsMatch(ac => ac.OfType<IEnumerable<GameCreatedEvent>>().Count() == 1);
        }

        [Test]
        public void AggregateServiceShouldDelegateToListenersOnCreate()
        {
            //Arrange
            var fakeDetails = A.Fake<GameDetailsDenormalizer>();
            var eventListener = new EventListner(new List<IProjection>{fakeDetails});
            var store = new DelegatingEventStore(new InMemoryEventStore(), new List<IEventListner>{eventListener});
            var appservice = new ApplicationService<Game>(store);

            //Act
            appservice.Handle(new CreateGameCommand(Guid.NewGuid(), Guid.NewGuid(), "test", 2));

            //Assert
            fakeDetails.CallsTo(gd => gd.Handle((GameCreatedEvent)null))
                .WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void AggregateServiceShouldDelegateToListenersOnStarted()
        {
            //Arrange
            var fakeDetails = A.Fake<GameDetailsDenormalizer>();
            var eventListener = new EventListner(new List<IProjection> { fakeDetails });
            var store = new DelegatingEventStore(new InMemoryEventStore(), new List<IEventListner> { eventListener });
            var appservice = new ApplicationService<Game>(store);

            //Act
            appservice.Handle(new CreateGameCommand(Guid.NewGuid(), Guid.NewGuid(), "test", 1));
            appservice.Handle(new JoinGameCommand(Guid.NewGuid(), Guid.NewGuid()));

            //Assert
            fakeDetails.CallsTo(gd => gd.Handle((GameStartedEvent)null))
                .WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
