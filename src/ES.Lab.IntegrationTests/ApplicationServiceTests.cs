using System;
using System.Collections.Generic;
using System.Linq;
using ES.Lab.Commands;
using ES.Lab.Domain;
using ES.Lab.Events;
using ES.Lab.Infrastructure.Data;
using ES.Lab.Read;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using Treefort.Common.Extensions;
using Treefort.Events;
using Treefort.Commanding;
using Treefort.Infrastructure;
using Treefort.Read;

namespace ES.Lab.IntegrationTests
{
    public class ApplicationServiceTests
    {
        private Func<ApplicationService<Game>> _appserviceFactory;
        private GameDetailsProjection _details;
        private OpenGamesProjection _openGames;
        private IEventStore _store;
        private IProjectionContext _projectionContext;

        [SetUp]
        public void Setup()
        {
            _projectionContext = new InMemoryProjectionContext();
            _details = new GameDetailsProjection(_projectionContext);
            _openGames = new OpenGamesProjection();

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
            _details = A.Fake<GameDetailsProjection>();
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
            _details = A.Fake<GameDetailsProjection>();
            _openGames = A.Fake<OpenGamesProjection>();
            var id = Guid.NewGuid();

            //Act, Assert
            PlayGame(d => _details.CallsTo(gd => gd.Handle((GameStartedEvent)null)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once),
                new CreateGameCommand(id, string.Empty, "test", 1),
                new JoinGameCommand(id, "tester@hotmail.com")
                );
        }

        [Test]
        public void EndToEndGamePlay()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var playerOne = "player1@jayway.com";
            var playerTwo = "player2@jayway.com";
            var commands = new List<ICommand>
                               {
                                   new CreateGameCommand(gameId, playerOne, "test", 3),
                                   new JoinGameCommand(gameId, playerTwo),
                                   new MakeChoiceCommand(gameId, playerOne, Choice.Paper),
                                   new MakeChoiceCommand(gameId, playerTwo, Choice.Scissors),
                                   new MakeChoiceCommand(gameId, playerOne, Choice.Rock),
                                   new MakeChoiceCommand(gameId, playerTwo, Choice.Paper),
                                   new MakeChoiceCommand(gameId, playerOne, Choice.Scissors),
                                   new MakeChoiceCommand(gameId, playerTwo, Choice.Paper)
                               };
            //Act, Assert
            PlayGame(d => Assert.AreEqual(playerTwo, d.WinnerId), commands.ToArray());
            
        }

        [Test]
        public void DrawShouldAddRound()
        {
            //Arrange
            var gameId = Guid.NewGuid();
            var playerOne = "player1@jayway.com";
            var playerTwo = "player2@jayway.com";
            var commands = new List<ICommand>
                               {
                                   new CreateGameCommand(gameId, playerOne, "test", 3),
                                   new JoinGameCommand(gameId, playerTwo),
                                   new MakeChoiceCommand(gameId, playerOne, Choice.Paper),
                                   new MakeChoiceCommand(gameId, playerTwo, Choice.Scissors),
                                   new MakeChoiceCommand(gameId, playerOne, Choice.Paper),
                                   new MakeChoiceCommand(gameId, playerTwo, Choice.Rock),
                                   new MakeChoiceCommand(gameId, playerOne, Choice.Scissors),
                                   new MakeChoiceCommand(gameId, playerTwo, Choice.Scissors)
                               };
            //Act, Assert
            PlayGame(d => Assert.AreEqual(4, d.Rounds.Count()), commands.ToArray());
        }

        private void PlayGame(Action<GameDetails> assert, params ICommand[] commands)
        {
            var appservice = _appserviceFactory();
            commands.ForEach(appservice.Handle);
            assert(_projectionContext.GameDetails.SingleOrDefault(x => x.GameId == commands.First().AggregateId));

        }
    }
}
