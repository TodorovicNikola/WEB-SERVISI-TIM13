using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TicketingSystem.DAL.Models
{
    public class Ticket
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketID { get; set; }

        [Key]
        [ForeignKey("Project")]
        [Column(Order = 1)]
        public int ProjectID { get; set; }

        [StringLength(64, ErrorMessage = "'Task Name' must not be longer than 64 characters!"), Required(ErrorMessage = "'Task Name' must not be empty!")]
        public String TaskName { get; set; }

        [StringLength(2048, ErrorMessage = "'Task Description' must not be longer than 2048 characters!")]
        public String TaskDescription { get; set; }

        [Required]
        public String TaskStatus { get; set; }

        [Required]
        public String TaskPriority { get; set; }

        public DateTime TaskCreated { get; set; }

        [Required(ErrorMessage = "'Task From' must not be empty!")]
        public DateTime TaskFrom { get; set; }

        [Required(ErrorMessage = "'Task Until' must not be empty!")]
        public DateTime TaskUntil { get; set; }

        [ForeignKey("UserCreated")]
        [Required]
        public string UserCreatedID { get; set; }

        [ForeignKey("UserAssigned")]
        public String UserAssignedID { get; set; }



        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        [ForeignKey("UserCreatedID")]
        [InverseProperty("CreatedTasks")]
        [JsonIgnore]
        public virtual TicketingSystemUser UserCreated { get; set; }

        [ForeignKey("UserAssignedID")]
        [InverseProperty("AssignedTasks")]
        [JsonIgnore]
        public virtual TicketingSystemUser UserAssigned { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Change> Changes { get; set; }
    }
}
