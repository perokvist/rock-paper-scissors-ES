using System.Data.Entity;
using System.Threading.Tasks;

namespace ES.Lab.Infrastructure.Data.Events
{
    public interface IEventContext
    {
        IDbSet<EventStream> Streams { get; set; }
        Task<int> SaveChangesAsync();
    }
}