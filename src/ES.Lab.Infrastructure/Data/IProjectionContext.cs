using System.Data.Entity;
using System.Threading.Tasks;
using ES.Lab.Read;

namespace ES.Lab.Infrastructure.Data
{
    public interface IProjectionContext : IReadContext
    {
        Task<int> SaveChangesAsync();
    }

    public interface IReadContext
    {
        IDbSet<GameDetails> GameDetails { get; }
        IDbSet<OpenGame> OpenGames { get; }
    }
}