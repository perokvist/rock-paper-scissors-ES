using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using ES.Lab.Commands;
using ES.Lab.Domain;
using ES.Lab.Events;
using ES.Lab.Infrastructure;
using ES.Lab.Infrastructure.Data;
using ES.Lab.Read;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using Treefort.EntityFramework.Eventing;
using Treefort.Events;
using Treefort.Commanding;
using Treefort.Infrastructure;
using Treefort.Read;
using System.Threading.Tasks;

namespace ES.Lab.IntegrationTests
{
    public class ApplicationServiceTests
    {
        private Func<ApplicationService<Game>> _appserviceFactory;
        private IProjection _details;
        private IProjection _openGames;
        private IEventStore _store;
        private IProjectionContext _projectionContext;

        [SetUp]
        public void Setup()
        {
            _projectionContext = new ProjectionContext(); //Possible to swap to InMemory, but DbAsync trouble
            _details = new GameDetailsProjection(_projectionContext);
            _openGames = new OpenGamesProjection(_projectionContext);
            var eventStoreFactory = new Lazy<IEventStore>(() =>
                                              {
                                                  var eventListener = new UnitOfWorkEventListener(new List<IProjection> { _details, _openGames }, _projectionContext);
                                                  return new DelegatingEventStore(new EventStore(
                                                      new EventContext(), x=> new EventStreamAdapter(x, new JsonConverterService())), 
                                                      new List<IEventListener> { eventListener });
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
            appservice.HandleAsync(new CreateGameCommand(Guid.NewGuid(), string.Empty, "test", 3)).Wait();

            //Assert
            _store.CallsTo(s => s.StoreAsync(Guid.NewGuid(), 0, null))
                .WhenArgumentsMatch(ac => ac.OfType<IEnumerable<GameCreatedEvent>>().Count() == 1);
        }

        [Test]
        public void AggregateServiceShouldDelegateToListenersOnCreate()
        {
            //Arrange
            _details = A.Fake<IProjection>();

            var appservice = _appserviceFactory();

            //Act
            appservice.HandleAsync(new CreateGameCommand(Guid.NewGuid(), string.Empty, "test", 2)).Wait();

            //Assert
            _details.CallsTo(gd => gd.WhenAsync((GameCreatedEvent)null))
                .WithAnyArguments().MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void AggregateServiceShouldDelegateToListenersOnStarted()
        {
            //Arrange
            _details = A.Fake<IProjection>();
            _openGames = A.Fake<IProjection>();

            var id = Guid.NewGuid();

            //Act, Assert
            PlayGame(d => _details.CallsTo(gd => gd.WhenAsync(null)).WithAnyArguments().MustHaveHappened(Repeated.Exactly.Times(3)),
                new CreateGameCommand(id, string.Empty, "test", 1),
                new JoinGameCommand(id, "tester@hotmail.com")
                );
        }

        //[Test, Ignore("IDbAsyncEnumerable trouble")]
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

        //[Test, Ignore("IDbAsyncEnumerable trouble")]
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

        

        private async void PlayGame(Action<GameDetails> assert, params ICommand[] commands)
        {
            var appservice = _appserviceFactory();
            foreach (var command in commands)
            {
                await appservice.HandleAsync(command);
            }
            //commands.ForEach(c => appservice.HandleAsync(c).Wait());
            var id = commands.First().AggregateId;
            assert(_projectionContext.GameDetails.SingleOrDefault(x => x.GameId == id));
        }

            
        
    }

    
    public class ProjectionConfigurator : FakeConfigurator<IProjection>
    {
        public override void ConfigureFake(IProjection fakeObject)
        {
            fakeObject.CallsTo(x => x.WhenAsync(null)).WithAnyArguments().Returns(Task.Run(() => {  }));
        }
    }

    

}
