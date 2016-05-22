namespace TicketingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Changes", "ChangeStatus", c => c.String());
            AlterColumn("dbo.Changes", "ChangePriority", c => c.String());
            AlterColumn("dbo.Tickets", "TaskStatus", c => c.String(nullable: false));
            AlterColumn("dbo.Tickets", "TaskPriority", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "TaskPriority", c => c.Int(nullable: false));
            AlterColumn("dbo.Tickets", "TaskStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Changes", "ChangePriority", c => c.Int());
            AlterColumn("dbo.Changes", "ChangeStatus", c => c.Int());
        }
    }
}
