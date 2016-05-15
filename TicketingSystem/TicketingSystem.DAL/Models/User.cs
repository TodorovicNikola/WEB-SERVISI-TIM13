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

        [StringLength(64, ErrorMessage = "'Username' must not be longer than 64 characters!"), Required(ErrorMessage = "'Username' must not be empty!")]
        public String Username { get; set; }

        [StringLength(64, ErrorMessage = "'Password' must not be longer than 64 characters!"), Required(ErrorMessage = "'Password' must not be empty!")]
        public String Password { get; set; }

        [StringLength(64, ErrorMessage = "'E-Mail' must not be longer than 64 characters!"), Required(ErrorMessage = "'E-Mail' must not be empty!")]
        public String EMail { get; set; }

        [StringLength(32, ErrorMessage = "'First Name' must not be longer than 32 characters!")]
        public String FirstName { get; set; }

        [StringLength(32, ErrorMessage = "'Last Name' must not be longer than 32 characters!")]
        public String LastName { get; set; }

        public ICollection<Project> AssignedProjects { get; set; }
        public ICollection<Task> CreatedTasks { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Change> ChangesCommited { get; set; }
    }
}
