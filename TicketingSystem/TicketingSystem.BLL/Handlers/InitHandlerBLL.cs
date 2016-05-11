using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.DAL.Handlers;

namespace TicketingSystem.BLL.Handlers
{
    public class InitHandlerBLL
    {
        InitHandlerDAL _iHDAL = new InitHandlerDAL();
        
        public void InitDB()
        {
            _iHDAL.InitDB();
        }

    }
}
