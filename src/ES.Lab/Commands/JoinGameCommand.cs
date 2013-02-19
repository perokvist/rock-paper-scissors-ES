using System;

namespace ES.Lab.Commands
{
    public class JoinGameCommand : ICommand
    {
        public JoinGameCommand(Guid entityId, Guid playerId)
        {
            PlayerId = playerId;
            EntityId = entityId;
        }

        public Guid PlayerId { get; private set; }
        public Guid EntityId { get; private set; }

    }
}