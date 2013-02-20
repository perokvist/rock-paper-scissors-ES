using System;
using ES.Lab.Domain;

namespace ES.Lab.Events
{
    public class ChoiceMadeEvent : IEvent
    {
        
        public ChoiceMadeEvent(Guid entityId, int round, string playerId, Choice choice)
        {
            EntityId = entityId;
            Round = round;
            PlayerId = playerId;
            Choice = choice;
        }

        public string PlayerId { get; private set; }
        public Guid EntityId { get; private set; }
        public int Round { get; private set; }
        public Choice Choice { get; private set; }
    }
}