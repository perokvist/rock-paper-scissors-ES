using System;
using System.Collections.Generic;

namespace ES.Lab
{
    public interface IEventStore
    {
        void Store(Guid entityId, long version, IEnumerable<IEvent> events);

        IEventStream LoadEventStream(Guid entityId);
    }
}