using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.DTOs
{
    public class ReportProjectTickets
    {
        public ProjectDTO Project { get; set; }
        public LinkedList<Tuple<UserDTO, Double>> percents { get; set; }
        public Double Unassigned { get; set; }
    }
}