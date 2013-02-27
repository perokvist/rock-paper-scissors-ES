namespace ES.Lab.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Title : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameDetails", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameDetails", "Title");
        }
    }
}
