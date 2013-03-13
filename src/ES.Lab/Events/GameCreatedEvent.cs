using System;
using Treefort.Events;

namespace ES.Lab.Events
{
    public class GameCreatedEvent : IEvent
    {
        
        public GameCreatedEvent(Guid entityId, string playerId, string title, int firstTo, DateTime utcNow)
        {
            GameId = entityId;
            PlayerId = playerId;
            Title = title;
            FirstTo = firstTo;
            Created = utcNow;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; set; }
        public string Title { get; set; }
        public int FirstTo { get; set; }
        public DateTime Created { get; set; }
        public string CorrelationId { get; set; }
    }
}
