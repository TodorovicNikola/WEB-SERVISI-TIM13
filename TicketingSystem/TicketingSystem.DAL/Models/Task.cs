using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DAL.Models
{
    public class Task
    {
        public int TaskID { get; set; }

        public String TaskName { get; set; }
        public String TaskDescription { get; set; }

        public enum TaskStatuses { ToDo, InProgress, Verify, Done };
        public TaskStatuses TaskStatus { get; set; }

        public enum TaskPriorities { Blocker, Critical, Major, Minor, Trivial };
        public TaskPriorities TaskPriority { get; set; }

        public DateTime TaskFrom { get; set; }
        public DateTime TaskUntil { get; set; }

    }
}
