using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.DTOs
{
    public class TaskDto
    {
        public int TicketId { get; set; }

        public String TaskName { get; set; }

        public String TaskDescription { get; set; }

        public String TaskStatus { get; set; }

        public String TaskPriority { get; set; }

        public DateTime TaskFrom { get; set; }

        public DateTime TaskUntil { get; set; }

        public String UserCreated { get; set; }

        public String UserAssigned { get; set; }

    }
}