using ES.Lab.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ES.Lab.Read
{
    public class OpenGamesDenormalizer : IProjection
    {
        private readonly IDictionary<Guid, OpenGame> _openGames;

        public OpenGamesDenormalizer()
        {
            _openGames = new ConcurrentDictionary<Guid, OpenGame>();
        }
    
        public void Handle(GameCreatedEvent @event)
        {
            _openGames.Add(@event.GameId, new OpenGame(@event.GameId, @event.PlayerId, @event.Created, @event.FirstTo));
        }

        public void Handle(GameStartedEvent @event)
        {
            _openGames.Remove(@event.GameId);
        }

        public IEnumerable<OpenGame> GetOpenGames()
        {
            return _openGames.Values;
        }

    }
}