using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TicketingSystem.DAL.Models;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace TicketingSystem.DAL
{
    public class TicketingSystemDBContext : DbContext
    {
        public TicketingSystemDBContext() : base("TicketingSystem") { }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Change> Changes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }
}
