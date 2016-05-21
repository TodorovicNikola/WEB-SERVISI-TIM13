using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace TicketingSystem.DAL.Models
{
    public class TicketingSystemDBContext : IdentityDbContext<TicketingSystemUser>
    {
        public TicketingSystemDBContext()
            : base("TicketingSystem")
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
//        public virtual DbSet<TicketingSystemUser> TicketingSystemUsers { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Change> Changes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        public static TicketingSystemDBContext Create()
        {
            return new TicketingSystemDBContext();
        }
    }
}