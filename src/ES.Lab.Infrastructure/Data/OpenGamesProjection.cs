using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using ES.Lab.Events;
using ES.Lab.Read;
using Treefort.Events;
using Treefort.Read;

namespace ES.Lab.Infrastructure.Data
{
    public class OpenGamesProjection : IgnoreNonApplicableEvents, IProjection
    {
        private readonly IProjectionContext _context;

        public OpenGamesProjection(IProjectionContext context)
        {
            _context = context;
        }

        public virtual void Handle(GameCreatedEvent @event)
        {
            _context.OpenGames.Add(new OpenGame(@event.GameId, @event.PlayerId, @event.Created, @event.FirstTo));
        }

        public virtual void Handle(GameStartedEvent @event)
        {
            Apply(@event.GameId, g => _context.OpenGames.Remove(g));
        }

        async void IProjection.When(IEvent @event)
        {
            this.Handle((dynamic)@event);
            int result = _context.SaveChangesAsync().Result;
        }

        private async void Apply(Guid id, Action<OpenGame> action)
        {
            var openGame = await _context.OpenGames.SingleOrDefaultAsync(g => g.GameId == id);
            if (openGame == null) return;
            action(openGame);
        }
    }
}