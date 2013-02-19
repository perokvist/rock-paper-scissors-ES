using System;

namespace ES.Lab.Domain
{
    public class ChoiceMadeEvent : IEvent
    {
        public ChoiceMadeEvent(Guid entityId, object round, string playerId, Choice choice)
        {
            throw new NotImplementedException();
        }
    }
}