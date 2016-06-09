namespace TicketingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Changes",
                c => new
                    {
                        ChangeID = c.Int(nullable: false, identity: true),
                        TaskID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        ChangeDate = c.DateTime(nullable: false),
                        ChangeName = c.String(),
                        ChangeDescription = c.String(),
                        ChangeStatus = c.String(),
                        ChangePriority = c.String(),
                        ChangeTaskFrom = c.DateTime(),
                        ChangeTaskUntil = c.DateTime(),
                        ChangeUserAssignedID = c.String(),
                        UserThatChangedID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ChangeID, t.TaskID, t.ProjectID })
                .ForeignKey("dbo.Tickets", t => new { t.TaskID, t.ProjectID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserThatChangedID)
                .Index(t => new { t.TaskID, t.ProjectID })
                .Index(t => t.UserThatChangedID);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        TaskName = c.String(nullable: false, maxLength: 64),
                        TaskDescription = c.String(maxLength: 2048),
                        TaskStatus = c.String(nullable: false),
                        TaskPriority = c.String(nullable: false),
                        TaskCreated = c.DateTime(nullable: false),
                        TaskFrom = c.DateTime(nullable: false),
                        TaskUntil = c.DateTime(nullable: false),
                        UserCreatedID = c.String(nullable: false, maxLength: 128),
                        UserAssignedID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TicketID, t.ProjectID })
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserAssignedID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserCreatedID)
                .Index(t => t.ProjectID)
                .Index(t => t.UserCreatedID)
                .Index(t => t.UserAssignedID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        TaskID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        CommentContent = c.String(nullable: false, maxLength: 1024),
                        CommentCreated = c.DateTime(nullable: false),
                        CommentUpdated = c.DateTime(),
                        UserWroteID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Tickets", t => new { t.TaskID, t.ProjectID })
                .ForeignKey("dbo.AspNetUsers", t => t.UserWroteID)
                .Index(t => new { t.TaskID, t.ProjectID })
                .Index(t => t.UserWroteID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserType = c.Int(nullable: false),
                        FirstName = c.String(maxLength: 32),
                        LastName = c.String(maxLength: 32),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 128),
                        ProjectCode = c.String(nullable: false, maxLength: 8),
                        ProjectDescription = c.String(maxLength: 2048),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        ReportType = c.Int(nullable: false),
                        Path = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ReportID, t.ProjectID })
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProjectTicketingSystemUsers",
                c => new
                    {
                        Project_ProjectID = c.Int(nullable: false),
                        TicketingSystemUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Project_ProjectID, t.TicketingSystemUser_Id })
                .ForeignKey("dbo.Projects", t => t.Project_ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TicketingSystemUser_Id, cascadeDelete: true)
                .Index(t => t.Project_ProjectID)
                .Index(t => t.TicketingSystemUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Changes", "UserThatChangedID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Changes", new[] { "TaskID", "ProjectID" }, "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "UserCreatedID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "UserAssignedID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Comments", "UserWroteID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reports", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectTicketingSystemUsers", "TicketingSystemUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectTicketingSystemUsers", "Project_ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Comments", new[] { "TaskID", "ProjectID" }, "dbo.Tickets");
            DropIndex("dbo.ProjectTicketingSystemUsers", new[] { "TicketingSystemUser_Id" });
            DropIndex("dbo.ProjectTicketingSystemUsers", new[] { "Project_ProjectID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Reports", new[] { "ProjectID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "UserWroteID" });
            DropIndex("dbo.Comments", new[] { "TaskID", "ProjectID" });
            DropIndex("dbo.Tickets", new[] { "UserAssignedID" });
            DropIndex("dbo.Tickets", new[] { "UserCreatedID" });
            DropIndex("dbo.Tickets", new[] { "ProjectID" });
            DropIndex("dbo.Changes", new[] { "UserThatChangedID" });
            DropIndex("dbo.Changes", new[] { "TaskID", "ProjectID" });
            DropTable("dbo.ProjectTicketingSystemUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Reports");
            DropTable("dbo.Projects");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Tickets");
            DropTable("dbo.Changes");
        }
    }
}
