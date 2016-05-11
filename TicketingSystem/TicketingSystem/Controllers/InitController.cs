using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.BLL.Handlers;

namespace TicketingSystem.Controllers
{
    public class InitController : ApiController
    {
        InitHandlerBLL _iHBLL = new InitHandlerBLL();

        public int GetInitDB()
        {
            _iHBLL.initDB();
            return 1;
        }
    }
}
