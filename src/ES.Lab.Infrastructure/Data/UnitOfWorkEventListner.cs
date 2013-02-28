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

        public UnitOfWorkEventListner(IEnumerable<IProjection> listerners)
        {
            _listerners = listerners;
        }

        public async Task ReceiveAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                foreach (var projection in _listerners)
                {
                    //TODO save and not wait
                    projection.WhenAsync(@event).Wait();
                }
            }
        }

    }
}