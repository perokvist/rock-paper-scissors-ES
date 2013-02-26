using ES.Lab.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Treefort.Events;
using Treefort.Read;

namespace ES.Lab.Read
{
    public class OpenGamesProjection : IgnoreNonApplicableEvents, IProjection, IOpenGamesView
    {
        private readonly IDictionary<Guid, OpenGame> _openGames;

        public OpenGamesProjection()
        {
            _openGames = new ConcurrentDictionary<Guid, OpenGame>();
        }
    
        public virtual void Handle(GameCreatedEvent @event)
        {
            _openGames.Add(@event.GameId, new OpenGame(@event.GameId, @event.PlayerId, @event.Created, @event.FirstTo));
        }

        public virtual void Handle(GameStartedEvent @event)
        {
            _openGames.Remove(@event.GameId);
        }

        public IEnumerable<OpenGame> GetOpenGames()
        {
            return _openGames.Values;
        }
        
        void IProjection.When(IEvent @event)
        {
            this.Handle((dynamic)@event);
        }
    }
}