using System;
using Treefort.Events;

namespace ES.Lab.Events
{
    public class RoundWonEvent : IEvent
    {

        public RoundWonEvent(Guid gameId, string playerId, int round)
        {
            GameId = gameId;
            PlayerId = playerId;
            Round = round;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; set; }
        public int Round { get; set; }
        public Guid CorrelationId { get; set; }
    }
}