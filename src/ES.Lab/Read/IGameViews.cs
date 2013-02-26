using System;
using System.Collections.Generic;

namespace ES.Lab.Read
{
    public interface IGameViews
    {
        GameDetails GetGameDetails(Guid gameId);
        IEnumerable<OpenGame> GetOpenGames();
    }
}