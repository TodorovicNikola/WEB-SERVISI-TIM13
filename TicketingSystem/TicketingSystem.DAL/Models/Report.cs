using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DAL.Models
{
    public class Report
    {
        public int RepartID { get; set; }

        public enum ReportTypes { AssignedTasks, FinishedTasks, DynamicsOfTaskCreations, DynamicsOfTaskFinishing, UserActivity };
        public ReportTypes ReportType { get; set; }

        public String Path { get; set; }
    }
}
