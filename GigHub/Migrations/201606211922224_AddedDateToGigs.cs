namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateToGigs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gigs", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gigs", "Date");
        }
    }
}
