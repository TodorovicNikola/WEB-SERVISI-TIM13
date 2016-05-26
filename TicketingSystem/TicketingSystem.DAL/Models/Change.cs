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

        public String ChangeName { get; set; }
        public String ChangeDescription { get; set; }
        public String ChangeStatus { get; set; }
        public String ChangePriority { get; set; }
        public DateTime? ChangeTaskFrom { get; set; }
        public DateTime? ChangeTaskUntil { get; set; }
        public string ChangeUserAssignedID { get; set; }

        [Required]
        [ForeignKey("UserThatChanged")]
        public string UserThatChangedID { get; set; }


        public virtual Ticket Task { get; set; }

        [JsonIgnore]
        [ForeignKey("UserThatChangedID")]
        public virtual TicketingSystemUser UserThatChanged { get; set; }

    }
}
