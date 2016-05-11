using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.DAL.Models
{
    public class Task
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        [Key]
        [ForeignKey("Project")]
        [Column(Order = 1)]
        public int ProjectID { get; set; }

        [StringLength(32), Required]
        public String TaskName { get; set; }

        [StringLength(2048)]
        public String TaskDescription { get; set; }

        public enum TaskStatuses { ToDo, InProgress, Verify, Done };
        [Required]
        public TaskStatuses TaskStatus { get; set; }

        public enum TaskPriorities { Blocker, Critical, Major, Minor, Trivial };
        [Required]
        public TaskPriorities TaskPriority { get; set; }

        [Required]
        public DateTime TaskFrom { get; set; }

        [Required]
        public DateTime TaskUntil { get; set; }

        [ForeignKey("UserCreated")]
        [Required]
        public int UserCreatedID { get; set; }

        [ForeignKey("UserAssigned")]
        public int? UserAssignedID { get; set; }



        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        [ForeignKey("UserCreatedID")]
        [InverseProperty("CreatedTasks")]
        public virtual User UserCreated { get; set; }

        [ForeignKey("UserAssignedID")]
        [InverseProperty("AssignedTasks")]
        public virtual User UserAssigned { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Change> Changes { get; set; }
    }
}
