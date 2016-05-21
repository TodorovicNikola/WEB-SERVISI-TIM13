using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.DTOs
{
    public class TaskDto
    {
        public int TaskId { get; set; }

        public String TaskName { get; set; }

        public String TaskDescription { get; set; }

        public DAL.Models.Ticket.TaskStatuses TaskStatus { get; set; }

        public DAL.Models.Ticket.TaskPriorities TaskPriority { get; set; }

        public DateTime TaskFrom { get; set; }

        public DateTime TaskUntil { get; set; }

        //public DAL.Models.User UserCreated { get; set; }

        // public DAL.Models.User UserAssigned { get; set; }
        public String UserCreated { get; set; }
        public String UserAssigned { get; set; }

    }
}