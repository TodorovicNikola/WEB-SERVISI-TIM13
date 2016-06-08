using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.DTOs
{
    public class ProjectTicketsDTO
    {
        public ProjectDTO Project { get; set; }
        public ICollection<Tuple<UserDTO, Double>> Users;
        public Double Unassigned;
    }
}