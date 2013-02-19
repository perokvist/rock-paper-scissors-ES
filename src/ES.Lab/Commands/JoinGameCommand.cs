using System;

namespace ES.Lab.Commands
{
    public class JoinGameCommand : ICommand
    {
        public Guid PlayerId { get; set; }

        public Guid EntityId { get; set; }
    }
}