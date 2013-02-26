using System;
using System.Collections.Generic;

namespace ES.Lab.Read
{
    public class GameViewsFacade : IGameViews
    {
        private readonly IGameDetailView _gameDetailView;
        private readonly IOpenGamesView _openGamesView;

        public GameViewsFacade(IGameDetailView gameDetailView, IOpenGamesView openGamesView)
        {
            _gameDetailView = gameDetailView;
            _openGamesView = openGamesView;
        }

        public GameDetails GetGameDetails(Guid gameId)
        {
            return _gameDetailView.GetGameDetails(gameId);
        }

        public IEnumerable<OpenGame> GetOpenGames()
        {
            return _openGamesView.GetOpenGames();
        } 
    }
}