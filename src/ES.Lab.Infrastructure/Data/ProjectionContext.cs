using System.Data.Entity;
using ES.Lab.Read;

namespace ES.Lab.Infrastructure.Data
{
    public class ProjectionContext : DbContext, IProjectionContext
    {
        public IDbSet<GameDetails> GameDetails { get; set; }
        public IDbSet<OpenGame> OpenGames { get; set; }
    }
}