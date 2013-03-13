using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treefort.Events;

namespace ES.Lab.Infrastructure.Signaling
{
    public interface IBroadcast
    {
        Task WhenAsync(IEvent @event);
    }
}
