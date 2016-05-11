using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TicketingSystem.DAL.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        [StringLength(128), Required]
        public String ProjectName { get; set; }

        [StringLength(8), Required]
        public String ProjectCode { get; set; }

        [StringLength(2048)]
        public String ProjectDescription { get; set; }


        public ICollection<Task> Tasks { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<User> AssignedUsers { get; set; }
    }
}
