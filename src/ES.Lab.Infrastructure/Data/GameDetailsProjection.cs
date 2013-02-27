using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ES.Lab.Events;
using ES.Lab.Read;
using Treefort.Events;
using Treefort.Read;
namespace ES.Lab.Infrastructure.Data
{
    public class GameDetailsProjection : IgnoreNonApplicableEvents, IProjection
    {
        private readonly IProjectionContext _context;

        public GameDetailsProjection(IProjectionContext context)
        {
            _context = context;
        }

        public virtual void Handle(GameCreatedEvent @event)
        {
            _context.GameDetails.Add(new GameDetails(@event.GameId, @event.PlayerId));
        }

        public virtual void Handle(GameStartedEvent @event)
        {
            Apply(@event.GameId, g => g.PlayerTwoId = @event.PlayerTwoId);
        }

        public virtual void Handle(RoundStartedEvent @event)
        {
            Apply(@event.GameId, g => g.AddRound(new Round(@event.Round)));
        }

        public virtual void Handle(ChoiceMadeEvent @event)
        {
            Apply(@event.GameId, g =>
            {
                var round = g.Rounds.Single(r => r.Number == @event.Round);
                if (g.PlayerOneId == @event.PlayerId)
                    round.PlayerOneHasMadeMove = true;
                else if (g.PlayerTwoId == @event.PlayerId)
                    round.PlayerTwoHasMadeMove = true;
            });
        }

        public virtual void Handle(GameWonEvent @event)
        {
            Apply(@event.GameId, g => g.WinnerId = @event.PlayerId);
        }

        async void IProjection.When(IEvent @event)
        {
            this.Handle((dynamic)@event);
            //TODO multitenent eventstore, ioc(EF), async
            await _context.SaveChangesAsync();
        }


        private async void Apply(Guid id, Action<GameDetails> action)
        {
            var gameDetails = await _context.GameDetails.SingleOrDefaultAsync(g => g.GameId == id);
            if (gameDetails == null) return;
            action(gameDetails);
        }

        
    }
}