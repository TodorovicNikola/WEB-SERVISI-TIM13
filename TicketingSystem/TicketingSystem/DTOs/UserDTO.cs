using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.DAL.Models;

namespace TicketingSystem.DTOs
{
    public class UserDTO
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }

        public UserDTO() { }

        public UserDTO(TicketingSystemUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}