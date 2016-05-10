using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DAL.Models
{
    public class Change
    {
        public int ChangeID { get; set; }
        public DateTime ChangeDate { get; set; }

        public String ChangeName { get; set; }
        public String ChangeDescription { get; set; }

        public Task.TaskStatuses ChangeStatus { get; set; }

        public Task.TaskPriorities ChangePriority { get; set; }

        public DateTime ChangeFrom { get; set; }
        public DateTime ChangeUntil { get; set; }
    }
}
