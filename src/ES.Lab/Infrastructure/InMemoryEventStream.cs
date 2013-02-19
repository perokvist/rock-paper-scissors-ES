using System.Collections.Generic;

namespace ES.Lab.Infrastructure
{
    public class InMemoryEventStream : List<IEvent>, IEventStream
    {
        public long Version { get; set; }
        public long EventCount
        {
            get { return base.Count; }
        }
    }
}