namespace ES.Lab.Infrastructure.ProjectionMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameDetails",
                c => new
                    {
                        GameId = c.Guid(nullable: false),
                        Title = c.String(),
                        PlayerOneId = c.String(),
                        PlayerTwoId = c.String(),
                        WinnerId = c.String(),
                    })
                .PrimaryKey(t => t.GameId);
            
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Number = c.Int(nullable: false),
                        PlayerOneHasMadeMove = c.Boolean(nullable: false),
                        PlayerTwoHasMadeMove = c.Boolean(nullable: false),
                        GameDetails_GameId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameDetails", t => t.GameDetails_GameId)
                .Index(t => t.GameDetails_GameId);
            
            CreateTable(
                "dbo.OpenGames",
                c => new
                    {
                        GameId = c.Guid(nullable: false),
                        PlayerId = c.String(),
                        Created = c.DateTime(nullable: false),
                        FirstTo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rounds", "GameDetails_GameId", "dbo.GameDetails");
            DropIndex("dbo.Rounds", new[] { "GameDetails_GameId" });
            DropTable("dbo.OpenGames");
            DropTable("dbo.Rounds");
            DropTable("dbo.GameDetails");
        }
    }
}
