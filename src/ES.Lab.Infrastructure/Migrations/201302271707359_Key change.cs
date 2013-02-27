namespace ES.Lab.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Keychange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GameDetails", "GameId", c => c.Guid(nullable: false));
            AlterColumn("dbo.OpenGames", "GameId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OpenGames", "GameId", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.GameDetails", "GameId", c => c.Guid(nullable: false, identity: true));
        }
    }
}
