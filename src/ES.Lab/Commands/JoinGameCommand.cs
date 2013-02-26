using System;

namespace ES.Lab.Commands
{
    public class JoinGameCommand : IGameCommand
    {
        public JoinGameCommand(Guid entityId, string playerId)
        {
            PlayerId = playerId;
            AggregateId = entityId;
        }

        public string PlayerId { get; private set; }
        public Guid AggregateId { get; private set; }

    }
}