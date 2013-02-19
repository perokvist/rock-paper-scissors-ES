using System;

namespace ES.Lab.Domain
{
    class GamePlayer
    {
        public GamePlayer(Guid playerId)
        {

        }

        public string Email { get; set; }

        public Choice CurrentChoice { get; set; }

        public decimal Score { get; set; }
    }
}
