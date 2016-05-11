using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.DAL.Models
{
    public class Change
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChangeID { get; set; }

        [Key]
        [ForeignKey("Task")]
        [Column(Order = 1)]
        public int TaskID { get; set; }

        [Key]
        [ForeignKey("Task")]
        [Column(Order = 2)]
        public int ProjectID { get; set; }

        [Required]
        public DateTime ChangeDate { get; set; }

        public Task.TaskStatuses? ChangeStatus { get; set; }
        public Task.TaskPriorities? ChangePriority { get; set; }
        public DateTime? ChangeTaskFrom { get; set; }
        public DateTime? ChangeTaskUntil { get; set; }

        [Required]
        [ForeignKey("UserThatChanged")]
        public int UserThatChangedID { get; set; }


        public virtual Task Task { get; set; }

        [ForeignKey("UserThatChangedID")]
        public virtual User UserThatChanged { get; set; }

    }
}
