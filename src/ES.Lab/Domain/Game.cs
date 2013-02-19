using System;
using System.Collections.Generic;
using ES.Lab.Commands;
using ES.Lab.Events;

namespace ES.Lab.Domain
{
    public class Game
    {
        private Guid id;
        private string title;
        private GamePlayer playerOne;
        private GamePlayer playerTwo;
        private int firstTo;

        public IEnumerable<IEvent> Handle(CreateGameCommand command)
        {
            return new List<IEvent>
                       {
                           new GameCreatedEvent(
                               command.EntityId,
                               command.PlayerId,
                               command.Title,
                               command.FirstTo,
                               DateTime.UtcNow)
                       };
        }

        public IEnumerable<IEvent> Handle(JoinGameCommand command)
        {
            var events = new List<IEvent>();

            if (playerTwo == null)
            {
                events.Add(new GameStartedEvent(id, playerOne.Email, command.PlayerId));
                events.Add(new RoundStartedEvent(id, 1));
            }

            return events;
        }
        
        public void Handle(GameCreatedEvent @event)
        {
            this.id = @event.GameId;
            this.title = @event.Title;
            this.playerOne = new GamePlayer(@event.PlayerId);
            this.firstTo = @event.FirstTo;
        }
    }
}
