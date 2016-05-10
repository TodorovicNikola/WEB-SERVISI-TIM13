using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DAL.Models
{
    public class User
    {
        public int UserID { get; set; }

        public enum UserTypes { ADMINISTRATOR, USER };
        public UserTypes UserType { get; set; }

        public String Username { get; set; }
        public String Password { get; set; }
        public String EMail { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}
