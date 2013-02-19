using System;

namespace ES.Lab.Events
{
    public class GameCreatedEvent : IEvent
    {
        
        public GameCreatedEvent(Guid entityId, Guid playerId, string title, int firstTo, DateTime utcNow)
        {
            GameId = entityId;
            PlayerId = playerId;
            Title = title;
            FirstTo = firstTo;
            UtcNow = utcNow;
        }

        public Guid GameId { get; private set; }
        public Guid PlayerId { get; private set; }
        public string Title { get; private set; }
        public int FirstTo { get; private set; }
        public DateTime UtcNow { get; private set; }

    }
}
