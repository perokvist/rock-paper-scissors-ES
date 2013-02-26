using System.Collections.Generic;

namespace ES.Lab.Read
{
    public interface IOpenGamesView
    {
        IEnumerable<OpenGame> GetOpenGames();
    }
}