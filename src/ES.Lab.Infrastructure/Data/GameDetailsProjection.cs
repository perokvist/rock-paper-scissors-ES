using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ES.Lab.Events;
using ES.Lab.Read;
using Treefort.Events;
using Treefort.Read;
namespace ES.Lab.Infrastructure.Data
{
    public class GameDetailsProjection : IgnoreNonApplicableEventsAsync, IProjection
    {
        private readonly IProjectionContext _context;

        public GameDetailsProjection(IProjectionContext context)
        {
            _context = context;
        }

        public virtual async Task Handle(GameCreatedEvent @event)
        {
            _context.GameDetails.Add(new GameDetails(@event.GameId, @event.Title, @event.PlayerId));
        }

        public virtual async Task Handle(GameStartedEvent @event)
        {
            await Apply(@event.GameId, g => g.PlayerTwoId = @event.PlayerTwoId);
        }

        public virtual async Task Handle(RoundStartedEvent @event)
        {
            await Apply(@event.GameId, g => g.AddRound(new Round { Number = @event.Round }));
        }


        public virtual async Task Handle(ChoiceMadeEvent @event)
        {
            await Apply(@event.GameId, g =>
            {
                var round = g.Rounds.Single(r => r.Number == @event.Round);
                if (g.PlayerOneId == @event.PlayerId)
                    round.PlayerOneHasMadeMove = true;
                else if (g.PlayerTwoId == @event.PlayerId)
                    round.PlayerTwoHasMadeMove = true;
            });
        }

        public virtual async Task Handle(GameWonEvent @event)
        {
            await Apply(@event.GameId, g => g.WinnerId = @event.PlayerId);
        }

        void IProjection.When(IEvent @event)
        {
            //TODO multitenent eventstore, ioc(EF), async
            ((Task) this.Handle((dynamic) @event)).Wait();
            var r = _context.SaveChangesAsync().Result;
        }

        private async Task Apply(Guid id, Action<GameDetails> action)
        {
            var gameDetails = await _context.GameDetails.SingleOrDefaultAsync(g => g.GameId == id);
            if (gameDetails == null) return;
            action(gameDetails);
        }
    }
}