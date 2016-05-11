using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.DAL.Models
{
    public class User
    {
        public int UserID { get; set; }

        public enum UserTypes { ADMINISTRATOR, USER };
        [Required]
        public UserTypes UserType { get; set; }

        [StringLength(64), Required]
        public String Username { get; set; }

        [StringLength(64), Required]
        public String Password { get; set; }

        [StringLength(64), Required]
        public String EMail { get; set; }

        [StringLength(32)]
        public String FirstName { get; set; }

        [StringLength(32)]
        public String LastName { get; set; }

        public ICollection<Project> AssignedProjects { get; set; }
        public ICollection<Task> CreatedTasks { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Change> ChangesCommited { get; set; }
    }
}
