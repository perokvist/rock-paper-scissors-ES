using System;
using ES.Lab.Domain;

namespace ES.Lab.Commands
{
    public class MakeChoiceCommand : ICommand
    {
        public MakeChoiceCommand(Guid gameId, String playerId, Choice choice)
        {
            EntityId = gameId;
            PlayerId = playerId;
            Choice = choice;
        }

        public Guid EntityId { get; private set; }
        public string PlayerId { get; private set; }
        public Choice Choice { get; private set; }

    }
}