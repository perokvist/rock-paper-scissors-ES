using System.Linq;
using Treefort.Events;

namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventStore : IEventStore
    {
        private readonly IEventContext _eventContext;

        public EventStore(IEventContext eventContext)
        {
            _eventContext = eventContext;
        }

        public IEventStream LoadEventStream(System.Guid entityId)
        {
           return new EventStreamAdapter(_eventContext.Streams.Single(e => e.AggregateId == entityId));
        }

        public async System.Threading.Tasks.Task StoreAsync(System.Guid entityId, long version, System.Collections.Generic.IEnumerable<IEvent> events)
        {
            var a = new EventStreamAdapter(_eventContext.Streams.Add(new EventStream() { AggregateId = entityId, Version = version }));
            a.AddRange(events);
            await _eventContext.SaveChangesAsync();
        }
    }
}