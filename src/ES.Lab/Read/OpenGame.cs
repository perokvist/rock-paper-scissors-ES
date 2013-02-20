using System;

namespace ES.Lab.Read
{
    public class OpenGame
    {
        
        public OpenGame(Guid gameId, string playerId, DateTime created, int firstTo)
        {
            GameId = gameId;
            PlayerId = playerId;
            Created = created;
            FirstTo = firstTo;
        }

        public Guid GameId { get; set; }
        public string PlayerId { get; set; }
        public DateTime Created { get; set; }
        public int FirstTo { get; set; }

    }
}