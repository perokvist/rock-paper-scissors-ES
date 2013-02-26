using System.Linq;
using ES.Lab.Read;
namespace ES.Lab.Infrastructure.Data
{
    public class GameViews : IGameViews
    {
        private readonly IReadContext _context;

        public GameViews(IReadContext context)
        {
            _context = context;
        }

        public GameDetails GetGameDetails(System.Guid gameId)
        {
            return _context.GameDetails.SingleOrDefault(g => g.GameId == gameId);
        }

        public System.Linq.IQueryable<OpenGame> GetOpenGames()
        {
            return _context.OpenGames;
        }
    }
}