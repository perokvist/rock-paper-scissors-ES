using System;
using System.Linq;
using Treefort.Events;

namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventStore : IEventStore
    {
        private readonly IEventContext _eventContext;
        private readonly Func<EventStream, IEventStream> _adapterFactory;

        public EventStore(IEventContext eventContext, Func<EventStream, IEventStream> adapterFactory)
        {
            _eventContext = eventContext;
            _adapterFactory = adapterFactory;
        }

        public IEventStream LoadEventStream(System.Guid entityId)
        {
            //TODO async + bootstappering , migration... event type deserialize
            return _adapterFactory(_eventContext.Streams.Single(e => e.AggregateId == entityId));
        }

        public async System.Threading.Tasks.Task StoreAsync(System.Guid entityId, long version, System.Collections.Generic.IEnumerable<IEvent> events)
        {
            var adapter = _adapterFactory(_eventContext.Streams.Add(new EventStream() { AggregateId = entityId, Version = version }));
            adapter.AddRange(events);
            await _eventContext.SaveChangesAsync();
        }
    }
}