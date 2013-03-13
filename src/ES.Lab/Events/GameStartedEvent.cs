using System;
using Treefort.Events;

namespace ES.Lab.Events
{
    public class GameStartedEvent : IEvent
    {
        public GameStartedEvent(Guid gameId, string playerOneId, string playerTwoId)
        {
            GameId = gameId;
            PlayerOneId = playerOneId;
            PlayerTwoId = playerTwoId;
        }

        public Guid GameId { get; set; }
        public string PlayerOneId { get; set; }
        public string PlayerTwoId { get; set; }
        public string CorrelationId { get; set; }
    }
}