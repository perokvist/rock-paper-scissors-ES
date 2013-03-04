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
        public string Json { get; set; }
    }
}