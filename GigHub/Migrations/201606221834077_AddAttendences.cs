namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttendences : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendences",
                c => new
                    {
                        GigId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GigId, t.UserId })
                .ForeignKey("dbo.Gigs", t => t.GigId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GigId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendences", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Attendences", "GigId", "dbo.Gigs");
            DropIndex("dbo.Attendences", new[] { "UserId" });
            DropIndex("dbo.Attendences", new[] { "GigId" });
            DropTable("dbo.Attendences");
        }
    }
}
