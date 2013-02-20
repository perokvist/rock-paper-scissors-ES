using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            if (!_gameDetails.ContainsKey(@event.GameId)) return;
            var gameDetails = _gameDetails[@event.GameId];
            gameDetails.PlayerTwoId = @event.PlayerTwoId;
        }

        public virtual void Handle(RoundStartedEvent @event)
        {
            if (!_gameDetails.ContainsKey(@event.GameId)) return;
            var gameDetails = _gameDetails[@event.GameId];
            gameDetails.AddRound(new Round(@event.Round));
        }

        public virtual void Handle(ChoiceMadeEvent @event)
        {
            if (!_gameDetails.ContainsKey(@event.GameId)) return;
            
            var gameDetails = _gameDetails[@event.GameId];
            var round = gameDetails.Rounds.Single(r => r.Number == @event.Round - 1);
            if(gameDetails.PlayerOneId == @event.PlayerId)
                round.PlayerOneHasMadeMove = true;
            else if(gameDetails.PlayerTwoId == @event.PlayerId)
                round.PlayerTwoHasMadeMove = true;
        }
   
        public virtual void Handle(GameWonEvent @event)
        {
            if(!_gameDetails.ContainsKey(@event.GameId)) return;
            var gameDetails = _gameDetails[@event.GameId];
            gameDetails.WinnerId = @event.PlayerId;
        }

        public GameDetails GetGameDetails(Guid gameId)
        {
            return _gameDetails[gameId];
        }
    
        public IEnumerable<GameDetails> AllGameDetails()
        {
            return _gameDetails.Values;
        }
    }
}