namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateGigTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Gigs", "ArtistId", "dbo.AspNetUsers");
            DropIndex("dbo.Gigs", new[] { "ArtistId" });
            AlterColumn("dbo.Gigs", "ArtistId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Gigs", "Venue", c => c.String(nullable: false));
            CreateIndex("dbo.Gigs", "ArtistId");
            AddForeignKey("dbo.Gigs", "ArtistId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gigs", "ArtistId", "dbo.AspNetUsers");
            DropIndex("dbo.Gigs", new[] { "ArtistId" });
            AlterColumn("dbo.Gigs", "Venue", c => c.String());
            AlterColumn("dbo.Gigs", "ArtistId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Gigs", "ArtistId");
            AddForeignKey("dbo.Gigs", "ArtistId", "dbo.AspNetUsers", "Id");
        }
    }
}
