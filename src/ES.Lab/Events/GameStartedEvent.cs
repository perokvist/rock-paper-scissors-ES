using System;

namespace ES.Lab.Events
{
    public class GameStartedEvent : IEvent
    {
        public GameStartedEvent(Guid gameId, string email, Guid playerId)
        {
            GameId = gameId;
            Email = email;
            PlayerId = playerId;
        }

        public Guid GameId { get; private set; }
        public string Email { get; private set; }
        public Guid PlayerId { get; private set; }

    }
}