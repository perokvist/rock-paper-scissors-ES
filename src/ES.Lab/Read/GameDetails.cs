using System;
using System.Collections.Generic;

namespace ES.Lab.Read
{
    public class GameDetails
    {
        protected GameDetails()
        {
            
        }

        public GameDetails(Guid gameId, string title, string playerOneId)
        {
            GameId = gameId;
            Title = title;
            PlayerOneId = playerOneId;
            Rounds = new List<Round>();
        }

        public Guid GameId { get; set; }
        public string Title { get; set; }

        public ICollection<Round> Rounds { get; set; }

        public string PlayerOneId { get; set; }

        public string PlayerTwoId { get; set; }

        public string WinnerId { get; set; }

        public void AddRound(Round round)
        {
            Rounds.Add(round);
        }
    }
}