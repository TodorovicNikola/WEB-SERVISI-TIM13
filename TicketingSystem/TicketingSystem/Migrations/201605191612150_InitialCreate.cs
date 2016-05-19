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
                    ChangeStatus = c.Int(),
                    ChangePriority = c.Int(),
                    ChangeTaskFrom = c.DateTime(),
                    ChangeTaskUntil = c.DateTime(),
                    UserThatChangedID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ChangeID, t.TaskID, t.ProjectID })
                .ForeignKey("dbo.Tasks", t => new { t.TaskID, t.ProjectID })
                .ForeignKey("dbo.Users", t => t.UserThatChangedID)
                .Index(t => new { t.TaskID, t.ProjectID })
                .Index(t => t.UserThatChangedID);

            CreateTable(
                "dbo.Tasks",
                c => new
                {
                    TaskID = c.Int(nullable: false, identity: true),
                    ProjectID = c.Int(nullable: false),
                    TaskName = c.String(nullable: false, maxLength: 64),
                    TaskDescription = c.String(maxLength: 2048),
                    TaskStatus = c.Int(nullable: false),
                    TaskPriority = c.Int(nullable: false),
                    TaskFrom = c.DateTime(nullable: false),
                    TaskUntil = c.DateTime(nullable: false),
                    UserCreatedID = c.Int(nullable: false),
                    UserAssignedID = c.Int(),
                })
                .PrimaryKey(t => new { t.TaskID, t.ProjectID })
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .ForeignKey("dbo.Users", t => t.UserAssignedID)
                .ForeignKey("dbo.Users", t => t.UserCreatedID)
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
                    UserWroteID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Tasks", t => new { t.TaskID, t.ProjectID })
                .ForeignKey("dbo.Users", t => t.UserWroteID)
                .Index(t => new { t.TaskID, t.ProjectID })
                .Index(t => t.UserWroteID);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserID = c.Int(nullable: false, identity: true),
                    UserType = c.Int(nullable: false),
                    Username = c.String(nullable: false, maxLength: 64),
                    Password = c.String(nullable: false, maxLength: 64),
                    EMail = c.String(nullable: false, maxLength: 64),
                    FirstName = c.String(maxLength: 32),
                    LastName = c.String(maxLength: 32),
                })
                .PrimaryKey(t => t.UserID);

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
                "dbo.ProjectUsers",
                c => new
                {
                    Project_ProjectID = c.Int(nullable: false),
                    User_UserID = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Project_ProjectID, t.User_UserID })
                .ForeignKey("dbo.Projects", t => t.Project_ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserID, cascadeDelete: true)
                .Index(t => t.Project_ProjectID)
                .Index(t => t.User_UserID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Changes", "UserThatChangedID", "dbo.Users");
            DropForeignKey("dbo.Changes", new[] { "TaskID", "ProjectID" }, "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "UserCreatedID", "dbo.Users");
            DropForeignKey("dbo.Tasks", "UserAssignedID", "dbo.Users");
            DropForeignKey("dbo.Tasks", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Comments", "UserWroteID", "dbo.Users");
            DropForeignKey("dbo.Reports", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectUsers", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.ProjectUsers", "Project_ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Comments", new[] { "TaskID", "ProjectID" }, "dbo.Tasks");
            DropIndex("dbo.ProjectUsers", new[] { "User_UserID" });
            DropIndex("dbo.ProjectUsers", new[] { "Project_ProjectID" });
            DropIndex("dbo.Reports", new[] { "ProjectID" });
            DropIndex("dbo.Comments", new[] { "UserWroteID" });
            DropIndex("dbo.Comments", new[] { "TaskID", "ProjectID" });
            DropIndex("dbo.Tasks", new[] { "UserAssignedID" });
            DropIndex("dbo.Tasks", new[] { "UserCreatedID" });
            DropIndex("dbo.Tasks", new[] { "ProjectID" });
            DropIndex("dbo.Changes", new[] { "UserThatChangedID" });
            DropIndex("dbo.Changes", new[] { "TaskID", "ProjectID" });
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.Reports");
            DropTable("dbo.Projects");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Tasks");
            DropTable("dbo.Changes");
        }
    }
}
