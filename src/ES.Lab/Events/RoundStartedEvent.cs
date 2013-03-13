using System;
using Treefort.Events;

namespace ES.Lab.Events
{
    public class RoundStartedEvent : IEvent
    {
        public RoundStartedEvent(Guid gameId, int round)
        {
            GameId = gameId;
            Round = round;
        }

        public Guid GameId { get; set; }
        public int Round { get; set; }
        public string CorrelationId { get; set; }
    }
}