using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using TicketingSystem.DAL.Models;
using System.Web.Http;
using System.Net.Http;

namespace TicketingSystem.Controllers
{
    [Authorize]
    public class FilterController : ApiController
    {
        private TicketingSystemDBContext db = new TicketingSystemDBContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        public async Task<IQueryable<Ticket>> PostFilter(List<String> filterIDs)
        {
            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            List<Ticket> _ticketList = new List<Ticket>();
            List<Ticket> _resultTicketList = new List<Ticket>();

            if (isAdmin)
            {
                if(filterIDs.Count == 0)
                {
                    return _resultTicketList.AsQueryable();
                }

                
                _ticketList.AddRange(db.Tickets);
            }
            else
            {
                List<Project> _projects = (from p in db.Projects.Include(p => p.AssignedUsers).Include(p => p.Tasks)
                                           where p.AssignedUsers.Any(u => u.Id == User.Identity.Name)
                                           select p).AsQueryable().ToList();

                foreach (Project _project in _projects)
                {
                    _ticketList.AddRange(_project.Tasks);
                }
            }


            foreach (string _filterID in filterIDs)
            {
                _resultTicketList.AddRange(_ticketList.Where(t => t.TaskPriority == _filterID));
            }

            return _resultTicketList.AsQueryable();

        }
    }
}