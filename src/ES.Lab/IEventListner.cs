using System.Collections.Generic;

namespace ES.Lab
{
    public interface IEventListner
    {
        void Receive(IEnumerable<IEvent> events);
    }
}