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
        public async Task<IQueryable<Project>> PostFilter(List<String> filterIDs)
        {
            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            if (isAdmin)
            {
                return db.Projects.Include(p => p.Tasks);
            }
            return (from p in db.Projects.Include(p => p.AssignedUsers)
                    where p.AssignedUsers.Any(u => u.Id == User.Identity.Name)
                    select p).AsQueryable();

        }
    }
}