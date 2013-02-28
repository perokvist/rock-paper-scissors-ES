using System.Collections.Generic;
using System.Linq;
using Treefort.Common.Extensions;
using Treefort.Events;
using Treefort.Read;

namespace ES.Lab.Infrastructure.Data
{
    public class AsyncEventListner : IEventListner
    {
        private readonly IEnumerable<IProjection> _listerners;

        public AsyncEventListner(IEnumerable<IProjection> listerners)
        {
            _listerners = listerners;
        }

        public void Receive(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                foreach (var projection in _listerners)
                {
                    projection.When(@event);
                }
            }
        }

    }
}