using Newtonsoft.Json;
using System.Linq;
using Treefort.Events;

namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventStreamAdapter : IEventStream
    {
        private readonly EventStream _eventStream;

        public EventStreamAdapter(EventStream eventStream)
        {
            _eventStream = eventStream;
        }

        public void AddRange(System.Collections.Generic.IEnumerable<IEvent> collection)
        {
            foreach (var @event in collection)
            {
                _eventStream.Events.Add(new Event(@event.ToJson()));
            }
        }

        public long EventCount
        {
            get { return _eventStream.Events.Count; }
        }

        public long Version
        {
            get { return _eventStream.Version; }
            set { _eventStream.Version = value; }
        }

        public void Add(IEvent item)
        {
            _eventStream.Events.Add(new Event(item.ToJson()));
        }

        public void Clear()
        {
            _eventStream.Events.Clear();
        }

        public bool Contains(IEvent item)
        {
            return _eventStream.Events.Contains((Event)item);
        }

        public void CopyTo(IEvent[] array, int arrayIndex)
        {
            _eventStream.Events.CopyTo((Event[])array, arrayIndex);
        }

        public int Count
        {
            get { return _eventStream.Events.Count; }
        }

        public bool IsReadOnly
        {
            get { return _eventStream.Events.IsReadOnly; }
        }

        public bool Remove(IEvent item)
        {
            return _eventStream.Events.Remove((Event)item);
        }

        public System.Collections.Generic.IEnumerator<IEvent> GetEnumerator()
        {
            return _eventStream.Events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _eventStream.Events.GetEnumerator();
        }
    }
}