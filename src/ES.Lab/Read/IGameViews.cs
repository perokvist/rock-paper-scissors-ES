using System;
using System.Collections.Generic;
using System.Linq;

namespace ES.Lab.Read
{
    public interface IGameViews
    {
        GameDetails GetGameDetails(Guid gameId);
        IQueryable<OpenGame> GetOpenGames();
    }
}