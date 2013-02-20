using System;

namespace ES.Lab.Commands
{
    public class CreateGameCommand : ICommand
    {
        public CreateGameCommand(Guid entityId, Guid playerId, string title, int firstTo)
        {
            EntityId = entityId;
            PlayerId = playerId;
            Title = title;
            FirstTo = firstTo;
        }

        public Guid EntityId { get; private set; }
        public Guid PlayerId { get; private set; }
        public string Title { get; private set; }
        public int FirstTo { get; private set; }
        
    }
}
