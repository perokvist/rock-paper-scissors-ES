using System;

namespace ES.Lab.Commands
{
    public class CreateGameCommand : IGameCommand
    {
        public CreateGameCommand(Guid entityId, string playerId, string title, int firstTo, Guid correlationId)
        {
            AggregateId = entityId;
            PlayerId = playerId;
            Title = title;
            FirstTo = firstTo;
            CorrelationId = correlationId;
        }

        public Guid AggregateId { get; private set; }
        public string PlayerId { get; private set; }
        public string Title { get; private set; }
        public int FirstTo { get; private set; }
        public Guid CorrelationId { get; private set; }
    }
}
