using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.DAL.Models
{
    public class Report
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportID { get; set; }

        [Key]
        [ForeignKey("Project")]
        [Column(Order = 1)]
        public int ProjectID { get; set; }

        public enum ReportTypes { AssignedTasks, FinishedTasks, DynamicsOfTaskCreations, DynamicsOfTaskFinishing, UserActivity };
        [Required]
        public ReportTypes ReportType { get; set; }

        [Required]
        [StringLength(128)]
        public String Path { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}
