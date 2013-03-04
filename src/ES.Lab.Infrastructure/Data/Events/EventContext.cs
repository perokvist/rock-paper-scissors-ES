using System.Data.Entity;
using Treefort.Events;
namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventContext : DbContext, IEventContext
    {
        public IDbSet<EventStream> Streams { get; set; }
    }

    public class Event : IEvent
    {
        protected Event()
        {
            
        }

        public Event(string json)
        {
            this.Json = json;
        }

        public string Json { get; set; }
    }
}