using System;
using ES.Lab.Domain;
using Treefort.Events;

namespace ES.Lab.Events
{
    public class ChoiceMadeEvent : IEvent
    {
        
        public ChoiceMadeEvent(Guid gameId, int round, string playerId, Choice choice)
        {
            GameId = gameId;
            Round = round;
            PlayerId = playerId;
            Choice = choice;
        }

        public string PlayerId { get; set; }
        public Guid GameId { get; set; }
        public int Round { get; set; }
        public Choice Choice { get; set; }
        public Guid CorrelationId { get; set; }
    }
}