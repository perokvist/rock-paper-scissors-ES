using System;
using System.Collections.Generic;

namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventStream 
    {
        public long Version { get; set; }
        public Guid AggregateId { get; set; }
        public ICollection<Event> Events { get; set; } 
    }
}