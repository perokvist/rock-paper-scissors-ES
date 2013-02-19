using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ES.Lab.Events;
namespace ES.Lab.Read
{
    public class GameDetailsDenormalizer : IProjection
    {
        private readonly IDictionary<Guid, GameDetails> _gameDetails;

        public GameDetailsDenormalizer()
        {
            _gameDetails = new ConcurrentDictionary<Guid, GameDetails>();
        }

        public virtual void Handle(GameCreatedEvent @event)
        {
            _gameDetails.Add(@event.GameId, new GameDetails(@event.GameId, @event.PlayerId));
        }

        public virtual void Handle(GameStartedEvent @event)
        {

        }

        public virtual void Handle(RoundStartedEvent @event)
        {

        }
    }
}