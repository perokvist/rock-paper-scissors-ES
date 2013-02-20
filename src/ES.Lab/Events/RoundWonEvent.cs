using System;

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

        public Guid GameId { get; private set; }
        public string PlayerId { get; private set; }
        public int Round { get; private set; }
    }
}