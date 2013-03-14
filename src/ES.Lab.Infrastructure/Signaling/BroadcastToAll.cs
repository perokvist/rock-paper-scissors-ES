using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Infrastructure;
using Treefort.Events;
namespace ES.Lab.Infrastructure.Signaling
{
    public class BroadcastToAll : IBroadcast
    {
        private readonly IConnectionManager _connectionManager;

        public BroadcastToAll(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public async Task WhenAsync(IEvent @event)
        {
            var hub = _connectionManager.GetHubContext("BroadcastHub");
            await hub.Clients.Client(@event.CorrelationId.ToString()).something(@event);
        }
    }
}