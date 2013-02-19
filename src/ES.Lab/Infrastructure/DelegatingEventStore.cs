using System.Collections.Generic;
using System.Linq;

namespace ES.Lab.Infrastructure
{
    /// <summary>
    /// Delegating Event Store Decorator
    /// </summary>
    public class DelegatingEventStore : IEventStore
    {
        private readonly IEventStore _store;
        private readonly IEnumerable<IEventListner> _eventListners;

        public DelegatingEventStore(IEventStore store, IEnumerable<IEventListner> eventListners)
        {
            _store = store;
            _eventListners = eventListners;
        }

        public void Store(System.Guid entityId, long version, IEnumerable<IEvent> events)
        {
            var enumerableEvents = events as IEvent[] ?? events.ToArray();
            _store.Store(entityId, version, enumerableEvents);
            _eventListners.ForEach(el => el.Receive(enumerableEvents));
        }

        public IEventStream LoadEventStream(System.Guid entityId)
        {
            return _store.LoadEventStream(entityId);
        }
    }
}