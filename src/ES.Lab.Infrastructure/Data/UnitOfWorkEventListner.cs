using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treefort.Common.Extensions;
using Treefort.Events;
using Treefort.Read;

namespace ES.Lab.Infrastructure.Data
{
    public class UnitOfWorkEventListner : IEventListner
    {
        private readonly IEnumerable<IProjection> _listerners;
        private readonly IProjectionContext _context;

        public UnitOfWorkEventListner(IEnumerable<IProjection> listerners, IProjectionContext context)
        {
            _listerners = listerners;
            _context = context;
        }

        public async Task ReceiveAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                foreach (var projection in _listerners)
                {
                    await projection.WhenAsync(@event);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}