using System;

namespace ES.Lab.Read
{
    public interface IGameDetailView
    {
        GameDetails GetGameDetails(Guid gameId);
    }
}