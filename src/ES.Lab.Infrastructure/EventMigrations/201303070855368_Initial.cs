namespace ES.Lab.Infrastructure.EventMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventStreams",
                c => new
                    {
                        AggregateId = c.Guid(nullable: false),
                        Version = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.AggregateId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Json = c.String(),
                        Type = c.String(),
                        EventStream_AggregateId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventStreams", t => t.EventStream_AggregateId)
                .Index(t => t.EventStream_AggregateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "EventStream_AggregateId", "dbo.EventStreams");
            DropIndex("dbo.Events", new[] { "EventStream_AggregateId" });
            DropTable("dbo.Events");
            DropTable("dbo.EventStreams");
        }
    }
}
