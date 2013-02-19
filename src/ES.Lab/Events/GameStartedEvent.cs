using System;

namespace ES.Lab.Events
{
    public class GameStartedEvent : IEvent
    {
        public GameStartedEvent(Guid id, string email, Guid playerId)
        {
        }
    }
}