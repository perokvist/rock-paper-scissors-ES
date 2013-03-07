using System.Threading.Tasks;
using ES.Lab.Read;
using Treefort.EntityFramework.Testing;

namespace ES.Lab.Infrastructure.Data
{
    public class InMemoryProjectionContext : IProjectionContext
    {
        public InMemoryProjectionContext()
        {
            GameDetails = new InMemoryDbSet<GameDetails>();
            OpenGames = new InMemoryDbSet<OpenGame>();
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.Factory.StartNew(() => 0);
        }

        public System.Data.Entity.IDbSet<Read.GameDetails> GameDetails { get; private set; }

        public System.Data.Entity.IDbSet<Read.OpenGame> OpenGames { get; private set; }
    }
}