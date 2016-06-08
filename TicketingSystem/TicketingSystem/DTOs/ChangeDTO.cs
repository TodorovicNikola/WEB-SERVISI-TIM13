using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.DAL.Models;

namespace TicketingSystem.DTOs
{
    public class ChangeDTO
    {

       
        public int ChangeID { get; set; }
        public int TaskID { get; set; }    
        public int ProjectID { get; set; }
        public DateTime ChangeDate { get; set; }
        public String ChangeName { get; set; }
        public String ChangeDescription { get; set; }
        public String ChangeStatus { get; set; }
        public String ChangePriority { get; set; }
        public DateTime? ChangeTaskFrom { get; set; }
        public DateTime? ChangeTaskUntil { get; set; }
        public string ChangeUserAssignedID { get; set; }
        public string UserThatChangedID { get; set; }

        public ChangeDTO(Change c)
        {
            this.ChangeID = c.ChangeID;
            this.TaskID = c.TaskID;
            this.ProjectID = c.ProjectID;
            this.ChangeDate = c.ChangeDate;
            this.ChangeName = c.ChangeName;
            this.ChangeDescription = c.ChangeDescription;
            this.ChangeStatus = c.ChangeStatus;
            this.ChangeTaskFrom = c.ChangeTaskFrom;
            this.ChangeTaskUntil = c.ChangeTaskUntil;
            this.ChangeUserAssignedID = c.ChangeUserAssignedID;
            this.UserThatChangedID = c.UserThatChangedID;
            this.ChangePriority = c.ChangePriority;
        }
    }
    
}