using System;
using ES.Lab.Domain;

namespace ES.Lab.Commands
{
    public class MakeChoiceCommand : IGameCommand
    {
        public MakeChoiceCommand(Guid gameId, String playerId, Choice choice)
        {
            AggregateId = gameId;
            PlayerId = playerId;
            Choice = choice;
        }

        public Guid AggregateId { get; private set; }
        public Guid CorrelationId { get; set; }
        public string PlayerId { get; private set; }
        public Choice Choice { get; private set; }

    }
}