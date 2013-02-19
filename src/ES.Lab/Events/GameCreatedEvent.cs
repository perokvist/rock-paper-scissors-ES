using System;

namespace ES.Lab.Events
{
    public class GameCreatedEvent : IEvent
    {
        public GameCreatedEvent(Guid entityId, Guid playerId, string title, int firstTo, DateTime utcNow)
        {

        }

        public Guid GameId { get; set; }

        public string Title { get; set; }

        public Guid PlayerId { get; set; }

        public int FirstTo { get; set; }
    }
}
