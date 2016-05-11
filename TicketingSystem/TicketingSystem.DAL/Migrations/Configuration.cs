namespace TicketingSystem.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TicketingSystem.DAL.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketingSystem.DAL.TicketingSystemDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketingSystem.DAL.TicketingSystemDBContext";
        }

        protected override void Seed(TicketingSystem.DAL.TicketingSystemDBContext context)
        {
            context.Users.AddOrUpdate(
                new User { Username = "admin", EMail = "admin@yahoo.com", FirstName = "Admin", LastName = "Adminovic", Password = "Admin", UserType = User.UserTypes.ADMINISTRATOR },
                new User { Username = "admin1", EMail = "admin1@yahoo.com", FirstName = "Admin1", LastName = "Adminovic1", Password = "Admin1", UserType = User.UserTypes.ADMINISTRATOR },
                new User { Username = "vlada", EMail = "vlada@yahoo.com", FirstName = "Vladimir", LastName = "Ivkovic", Password = "vlada", UserType = User.UserTypes.USER },
                new User { Username = "aleksa", EMail = "aleksa@yahoo.com", FirstName = "Aleksa", LastName = "Mirkovic", Password = "aleksa", UserType = User.UserTypes.USER },
                new User { Username = "nikola", EMail = "nikola@yahoo.com", FirstName = "Nikola", LastName = "Todorovic", Password = "nikola", UserType = User.UserTypes.USER },
                new User { Username = "petPet", EMail = "petPet@yahoo.com", FirstName = "Petar", LastName = "Petrovic", Password = "petPet", UserType = User.UserTypes.USER },
                new User { Username = "jovJov", EMail = "jovJov@yahoo.com", FirstName = "Jovan", LastName = "Jovanovic", Password = "jovJov", UserType = User.UserTypes.USER },
                new User { Username = "milMil", EMail = "milMil@yahoo.com", FirstName = "Milos", LastName = "Milosevic", Password = "milMil", UserType = User.UserTypes.USER },
                new User { Username = "nikNik", EMail = "nikNik@yahoo.com", FirstName = "Nikola", LastName = "Nikolic", Password = "nikNik", UserType = User.UserTypes.USER },
                new User { Username = "stefStef", EMail = "stefStef@yahoo.com", FirstName = "Stefan", LastName = "Stefanovic", Password = "stefStef", UserType = User.UserTypes.USER }
            );

            context.SaveChanges();

            context.Projects.AddOrUpdate(
                new Project { ProjectName = "Web Servisi", ProjectCode = "WS", ProjectDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." },
                new Project { ProjectName = "XML", ProjectCode = "XML", ProjectDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." },
                new Project { ProjectName = "Inzenjering informacionih sistema", ProjectCode = "IIS", ProjectDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur." },
                new Project { ProjectName = "Sistemi baza podataka", ProjectCode = "SBP", ProjectDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." },
                new Project { ProjectName = "Diplomski", ProjectCode = "DPL", ProjectDescription = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam." }
            );

            context.SaveChanges();

            context.Tasks.AddOrUpdate(
                new Task { ProjectID = 1, TaskName = "Projekat", TaskDescription = "Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today, TaskUntil = DateTime.Today.AddDays(5), TaskPriority = Task.TaskPriorities.Blocker, TaskStatus = Task.TaskStatuses.InProgress, UserCreatedID = 1, UserAssignedID = 4 },
                new Task { ProjectID = 1, TaskName = "Usmeni", TaskDescription = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", TaskFrom = DateTime.Today, TaskUntil = DateTime.Today.AddDays(8), TaskPriority = Task.TaskPriorities.Critical, TaskStatus = Task.TaskStatuses.Verify, UserCreatedID = 2 },
                new Task { ProjectID = 2, TaskName = "Projekat", TaskDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", TaskFrom = DateTime.Today.AddDays(5), TaskUntil = DateTime.Today.AddDays(10), TaskPriority = Task.TaskPriorities.Blocker, TaskStatus = Task.TaskStatuses.ToDo, UserCreatedID = 1, UserAssignedID = 3 },
                new Task { ProjectID = 2, TaskName = "Usmeni", TaskDescription = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", TaskFrom = DateTime.Today.AddDays(15), TaskUntil = DateTime.Today.AddDays(25), TaskPriority = Task.TaskPriorities.Critical, TaskStatus = Task.TaskStatuses.ToDo, UserCreatedID = 2 },
                new Task { ProjectID = 3, TaskName = "Projekat Programerski", TaskDescription = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", TaskFrom = DateTime.Today.AddDays(10), TaskUntil = DateTime.Today.AddDays(18), TaskPriority = Task.TaskPriorities.Blocker, TaskStatus = Task.TaskStatuses.InProgress, UserCreatedID = 1, UserAssignedID = 5 },
                new Task { ProjectID = 3, TaskName = "Projekat Menadzerski 1", TaskDescription = "Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today.AddDays(6), TaskUntil = DateTime.Today.AddDays(8), TaskPriority = Task.TaskPriorities.Major, TaskStatus = Task.TaskStatuses.ToDo, UserCreatedID = 1 },
                new Task { ProjectID = 3, TaskName = "Projekat Menadzerski 2", TaskDescription = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam.", TaskFrom = DateTime.Today.AddDays(8), TaskUntil = DateTime.Today.AddDays(10), TaskPriority = Task.TaskPriorities.Minor, TaskStatus = Task.TaskStatuses.ToDo, UserCreatedID = 2, UserAssignedID = 3 },
                new Task { ProjectID = 3, TaskName = "Specifikacija zahteva", TaskDescription = "Opsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today.AddDays(-30), TaskUntil = DateTime.Today.AddDays(-20), TaskPriority = Task.TaskPriorities.Critical, TaskStatus = Task.TaskStatuses.Done, UserCreatedID = 1 },
                new Task { ProjectID = 4, TaskName = "Kolokvijum", TaskDescription = "Veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today.AddDays(8), TaskUntil = DateTime.Today.AddDays(8), TaskPriority = Task.TaskPriorities.Critical, TaskStatus = Task.TaskStatuses.ToDo, UserCreatedID = 2 },
                new Task { ProjectID = 4, TaskName = "Projekat", TaskDescription = "Lo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today.AddDays(8), TaskUntil = DateTime.Today.AddDays(10), TaskPriority = Task.TaskPriorities.Blocker, TaskStatus = Task.TaskStatuses.InProgress, UserCreatedID = 1 },
                new Task { ProjectID = 4, TaskName = "Usmeni II", TaskDescription = "Eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today, TaskUntil = DateTime.Today.AddDays(5), TaskPriority = Task.TaskPriorities.Blocker, TaskStatus = Task.TaskStatuses.InProgress, UserCreatedID = 2, UserAssignedID = 5 },
                new Task { ProjectID = 5, TaskName = "Prijava", TaskDescription = "Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur.", TaskFrom = DateTime.Today.AddDays(35), TaskUntil = DateTime.Today.AddDays(45), TaskPriority = Task.TaskPriorities.Minor, TaskStatus = Task.TaskStatuses.Verify, UserCreatedID = 1 },
                new Task { ProjectID = 5, TaskName = "Izrada", TaskDescription = "Inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.", TaskFrom = DateTime.Today, TaskUntil = DateTime.Today.AddDays(5), TaskPriority = Task.TaskPriorities.Critical, TaskStatus = Task.TaskStatuses.InProgress, UserCreatedID = 1 },
                new Task { ProjectID = 5, TaskName = "Odbrana", TaskDescription = "Autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur", TaskFrom = DateTime.Today, TaskUntil = DateTime.Today.AddDays(5), TaskPriority = Task.TaskPriorities.Blocker, TaskStatus = Task.TaskStatuses.ToDo, UserCreatedID = 2, UserAssignedID = 4}
            );

            context.SaveChanges();
        }
    }
}
