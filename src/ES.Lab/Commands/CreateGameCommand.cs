using System;

namespace ES.Lab.Commands
{
    public class CreateGameCommand : ICommand
    {
        public Guid EntityId { get; private set; }

        public Guid PlayerId { get; private set; }

        public string Title { get; private set; }

        public int FirstTo { get; set; }
        
    }
}
