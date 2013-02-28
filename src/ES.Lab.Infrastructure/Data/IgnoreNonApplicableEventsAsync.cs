using System.Threading.Tasks;
using Treefort.Events;
namespace ES.Lab.Infrastructure.Data
{
    public abstract class IgnoreNonApplicableEventsAsync
    {
        public async Task Handle(IEvent @event)
        {

        }

    }
}