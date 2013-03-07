using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
            //TODO async + bootstappering , migration... move to treefort
            var stream = _eventContext.Streams.SingleOrDefault(e => e.AggregateId == entityId) ??
                         new EventStream() { Events = new Collection<Event>()};
            return _adapterFactory(stream);
        }

        public async Task StoreAsync(System.Guid entityId, long version, System.Collections.Generic.IEnumerable<IEvent> events)
        {
            var stream = _eventContext.Streams.SingleOrDefault(s => s.AggregateId == entityId) ??
                         _eventContext.Streams.Add(new EventStream() {AggregateId = entityId});
            var adapter = _adapterFactory(stream);
            adapter.Version = version;
            adapter.AddRange(events);
            await _eventContext.SaveChangesAsync();
        }
    }
}