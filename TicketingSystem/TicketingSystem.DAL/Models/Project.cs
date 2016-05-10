using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DAL.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public String ProjectName { get; set; }
        public String ProjectCode { get; set; }
        public String ProjectDescription { get; set; }
    }
}
