using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Treefort.Events;
namespace ES.Lab.Infrastructure.Signaling
{
    public class BroadcastingEventListener : IEventListener
    {
        private readonly IEnumerable<IBroadcast> _broadcasts;
        private readonly IConnectionManager _connectionManager;

        public BroadcastingEventListener(IEnumerable<IBroadcast> broadcasts, IConnectionManager connectionManager)
        {
            _broadcasts = broadcasts;
            _connectionManager = connectionManager;
        }

        public async Task ReceiveAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                foreach (var broadcast in _broadcasts)
                {
                    await broadcast.WhenAsync(@event);
                }
            }
        }
    }
}