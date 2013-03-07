using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ES.Lab.Infrastructure.Data.Events
{
    public class EventContext : DbContext, IEventContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventStream>()
             .HasKey(es => es.AggregateId);
            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id).Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public IDbSet<EventStream> Streams { get; set; }
    }
}