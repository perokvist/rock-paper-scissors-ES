using ES.Lab.Commands;

namespace ES.Lab.Infrastructure
{
    public class ApplicationService<TAggregate> : IApplicationService
        where TAggregate : new()
    {
        private readonly IEventStore _eventStore;

        public ApplicationService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(ICommand command)
        {
            //TODO route commands
            if (!(command is IGameCommand)) return;

            //Load events
            var eventStream = _eventStore.LoadEventStream(command.EntityId);
            //Instantiate aggregate
            var aggregate = new TAggregate() as dynamic;
            //Replay events
            eventStream.ForEach(e => aggregate.Handle((dynamic) e));
            //Execute command
            var events = aggregate.Handle((dynamic)command);
            //Store events
            _eventStore.Store(command.EntityId, eventStream.Version + 1, events);
        }
    }


}