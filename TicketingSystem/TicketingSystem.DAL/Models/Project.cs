using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.DAL.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        [StringLength(128, ErrorMessage = "'Task Name' must not be longer than 128 characters!"), Required(ErrorMessage = "'Project Name' must not be empty!")]
        public String ProjectName { get; set; }

        [Index(IsUnique = true)]
        [StringLength(8, ErrorMessage = "'Project Code' must not be longer than 8 characters!"), Required(ErrorMessage = "'Project Code' must not be empty!")]
        public String ProjectCode { get; set; }

        [StringLength(2048, ErrorMessage = "'Project Description' must not be longer than 2048 characters!")]
        public String ProjectDescription { get; set; }


        public ICollection<Ticket> Tasks { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<TicketingSystemUser> AssignedUsers { get; set; }
    }
}
