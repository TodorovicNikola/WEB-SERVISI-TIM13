using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.DAL.Models;

namespace TicketingSystem.DTOs
{
    public class TaskDto
    {
        public int TicketId { get; set; }

        public String TaskName { get; set; }

        public String TaskDescription { get; set; }

        public String TaskStatus { get; set; }

        public String TaskPriority { get; set; }

        public DateTime TaskCreated { get; set; }

        public DateTime TaskFinished { get; set; }

        public DateTime TaskUntil { get; set; }

        public String UserCreated { get; set; }

        public String UserAssigned { get; set; }

        public int TaskNumber { get; set; }

        public String ProjectName { get; set; }
        public String ProjectCode { get; set; }
        public TaskDto()
        {

        }
        public TaskDto(Ticket t)
        {
            this.TicketId = t.TicketID;
            this.TaskName = t.TaskName;
            this.TaskDescription = t.TaskDescription;
            this.TaskStatus = t.TaskStatus;
            this.TaskPriority = t.TaskPriority;
            this.TaskFinished = t.TaskFrom;
            this.TaskUntil = t.TaskUntil;
            this.UserAssigned = t.UserAssignedID;
            this.UserCreated = t.UserCreatedID;
            this.TaskCreated = t.TaskCreated;
            this.TaskNumber = t.TaskNumber;
            if (t.Project != null)
            {
                ProjectName = t.Project.ProjectName;
                ProjectCode = t.Project.ProjectCode;
            }
           
        }
    }

}