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

        public Guid GameId { get; private set; }
        public int Round { get; private set; }
    }
}