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
        private Func<ApplicationService<Game>> _appserviceFactory;
        private GameDetailsDenormalizer _details;
        private OpenGamesDenormalizer _openGames;
        private IEventStore _store;

        [SetUp]
        public void Setup()
        {
            _details = new GameDetailsDenormalizer();
            _openGames = new OpenGamesDenormalizer();

            var eventStoreFactory = new Lazy<IEventStore>(() =>
                                              {
                                                  var eventListener = new EventListner(new List<IProjection> { _details, _openGames });
                                                  return new DelegatingEventStore(new InMemoryEventStore(), new List<IEventListner> { eventListener });
                                              });

            _appserviceFactory = () => new ApplicationService<Game>(eventStoreFactory.Value);
        }

        [Test]
        public void AggregateServiceShouldStoreEvent()
        {
            //Arrange
            _store = A.Fake<IEventStore>();
            var appservice = _appserviceFactory();

            //Act
            appservice.Handle(new CreateGameCommand(Guid.NewGuid(), string.Empty, "test", 3));

            //Assert
            _store.CallsTo(s => s.Store(Guid.NewGuid(), 0, null))
                .WhenArgumentsMatch(ac => ac.OfType<IEnumerable<GameCreatedEvent>>().Count() == 1);
        }

        [Test]
        public void AggregateServiceShouldDelegateToListenersOnCreate()
        {
            //Arrange
            _details = A.Fake<GameDetailsDenormalizer>();
            var appservice = _appserviceFactory();

            //Act
            appservice.Handle(new CreateGameCommand(Guid.NewGuid(), string.Empty, "test", 2));

            //Assert
            _details.CallsTo(gd => gd.Handle((GameCreatedEvent)null))
                .WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void AggregateServiceShouldDelegateToListenersOnStarted()
        {
            //Arrange
            _details = A.Fake<GameDetailsDenormalizer>();
            _openGames = A.Fake<OpenGamesDenormalizer>();
            var appservice = _appserviceFactory();

            //Act
            var id = Guid.NewGuid();
            appservice.Handle(new CreateGameCommand(id, string.Empty, "test", 1));
            appservice.Handle(new JoinGameCommand(id, "tester@hotmail.com"));

            //Assert
            _details.CallsTo(gd => gd.Handle((GameStartedEvent)null))
                .WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void EndToEndGamePlay()
        {
            //Arrange
            _details = new GameDetailsDenormalizer();
            _openGames = new OpenGamesDenormalizer();
            var id = Guid.NewGuid();
            var playerOne = "player1@jayway.com";
            var playerTwo = "player2@jayway.com";
            var appservice = _appserviceFactory();

            //Act
            appservice.Handle(new CreateGameCommand(id, playerOne, "test", 3));
            appservice.Handle(new JoinGameCommand(id, playerTwo));
            appservice.Handle(new MakeChoiceCommand(id, playerOne, Choice.Paper));
            appservice.Handle(new MakeChoiceCommand(id, playerTwo, Choice.Scissors));
            appservice.Handle(new MakeChoiceCommand(id, playerOne, Choice.Rock));
            appservice.Handle(new MakeChoiceCommand(id, playerTwo, Choice.Paper));
            appservice.Handle(new MakeChoiceCommand(id, playerOne, Choice.Scissors));
            appservice.Handle(new MakeChoiceCommand(id, playerTwo, Choice.Paper));


            var result = _details.GetGameDetails(id);
           
            //Assert
            Assert.AreEqual(playerTwo, result.WinnerId);
        }
    }
}
