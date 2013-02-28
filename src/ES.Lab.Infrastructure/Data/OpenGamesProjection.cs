using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ES.Lab.Events;
using ES.Lab.Read;
using Treefort.Events;
using Treefort.Read;

namespace ES.Lab.Infrastructure.Data
{
    public class OpenGamesProjection : IgnoreNonApplicableEventsAsync, IProjection
    {
        private readonly IProjectionContext _context;

        public OpenGamesProjection(IProjectionContext context)
        {
            _context = context;
        }

        public virtual async Task Handle(GameCreatedEvent @event)
        {
            _context.OpenGames.Add(new OpenGame(@event.GameId, @event.PlayerId, @event.Created, @event.FirstTo));
        }

        public virtual async Task Handle(GameStartedEvent @event)
        {
            await Apply(@event.GameId, g => _context.OpenGames.Remove(g));
        }


        void IProjection.When(IEvent @event)
        {
            ((Task)this.Handle((dynamic)@event)).Wait();
            var r = _context.SaveChangesAsync().Result;
        }

        private async Task Apply(Guid id, Action<OpenGame> action)
        {
            var openGame = await _context.OpenGames.SingleOrDefaultAsync(g => g.GameId == id);
            if (openGame == null) return;
            action(openGame);
        }
    }
}