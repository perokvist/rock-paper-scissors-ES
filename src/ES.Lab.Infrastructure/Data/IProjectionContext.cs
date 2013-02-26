using System.Data.Entity;
using System.Threading.Tasks;
using ES.Lab.Read;

namespace ES.Lab.Infrastructure.Data
{
    public interface IProjectionContext
    {
        IDbSet<GameDetails> GameDetails { get; set; }
        IDbSet<OpenGame> OpenGames { get; set; }
        Task<int> SaveChangesAsync();
    }
}