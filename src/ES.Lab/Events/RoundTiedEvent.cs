using System;
using Treefort.Events;

namespace ES.Lab.Events
{
    public class RoundTiedEvent : IEvent
    {
        public RoundTiedEvent(Guid gameId, int round)
        {
            GameId = gameId;
            Round = round;
        }

        public Guid GameId { get; set; }
        public int Round { get; set; }
        public Guid CorrelationId { get; set; }
    }
}