using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Config;
using ES.Lab.Read;

namespace ES.Lab.Infrastructure.Data
{
    //[DbConfigurationType(typeof(ProjectionContextConfiguration))]
    public class ProjectionContext : DbContext, IProjectionContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameDetails>()
                .HasKey(g => g.GameId);
            modelBuilder.Entity<OpenGame>()
                .HasKey(g => g.GameId);
            modelBuilder.Entity<Round>()
                .HasKey(r => r.Id);
        }

        public IDbSet<GameDetails> GameDetails { get; set; }
        public IDbSet<OpenGame> OpenGames { get; set; }
    }
}