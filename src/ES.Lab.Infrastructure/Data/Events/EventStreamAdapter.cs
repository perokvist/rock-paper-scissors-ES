using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Linq;
using Treefort.Events;

namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventStreamAdapter : Collection<IEvent>, IEventStream
    {
        private readonly EventStream _eventStream;

        public EventStreamAdapter(EventStream eventStream)
        {
            _eventStream = eventStream;
            this.AddRange(_eventStream.Events
                .Select(e => JsonConvert.DeserializeObject(e.Json, e.Type))
                .Cast<IEvent>());
        }

        protected override void InsertItem(int index, IEvent item)
        {
            base.InsertItem(index, item);
            _eventStream.Events.Add(new Event(item.ToJson(), item.GetType()));
        }

        protected override void SetItem(int index, IEvent item)
        {
            base.SetItem(index, item);
            _eventStream.Events.Add(new Event(item.ToJson(), item.GetType()));
        }

        public long EventCount
        {
            get { return Count; }
        }

        public long Version
        {
            get { return _eventStream.Version; }
            set { _eventStream.Version = value; }
        }

        public void AddRange(IEnumerable<IEvent> collection)
        {
            var i = 0;
            foreach (var @event in collection)
            {
                SetItem(i, @event);
                i++;
            }
        }
    }
}