using System;

namespace ES.Lab.Read
{
    public class GameDetails
    {
        
        public GameDetails(Guid gameId, Guid playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }

    }
}