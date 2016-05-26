namespace TicketingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Changes", "ChangeName", c => c.String());
            AddColumn("dbo.Changes", "ChangeDescription", c => c.String());
            AddColumn("dbo.Changes", "ChangeUserAssignedID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Changes", "ChangeUserAssignedID");
            DropColumn("dbo.Changes", "ChangeDescription");
            DropColumn("dbo.Changes", "ChangeName");
        }
    }
}
