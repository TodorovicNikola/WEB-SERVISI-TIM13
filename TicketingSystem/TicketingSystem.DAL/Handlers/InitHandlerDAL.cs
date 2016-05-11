using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DAL.Models;

namespace TicketingSystem.DAL.Handlers
{
    public class InitHandlerDAL
    {
        TicketingSystemDBContext _tSDBC = new TicketingSystemDBContext();

        public void InitDB()
        {
            _tSDBC.Projects.Add(new Project() { ProjectName = "hasan", ProjectCode = "HSN" });
        }
    }
}
