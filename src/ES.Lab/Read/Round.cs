using System;

namespace ES.Lab.Read
{
    public class Round
    {
        public Round()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int Number { get; set; }

        public bool PlayerOneHasMadeMove { get; set; }

        public bool PlayerTwoHasMadeMove { get; set; }
    }
}