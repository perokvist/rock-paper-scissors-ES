using System;

namespace ES.Lab.Domain
{
    class GamePlayer
    {
        public GamePlayer(string email)
        {
            Email = email;
        }

        public string Email { get; set; }

        public Choice CurrentChoice { get; set; }

        public decimal Score { get; set; }

        public void AddWin()
        {
            Score += 1;
        }
    }
}
