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
using TicketingSystem.DTOs;

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
        public async Task<IQueryable<TaskDto>> PostFilter(List<String> filterIDs)
        {
            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            List<Ticket> _ticketList = new List<Ticket>();
            List<Ticket> _resultTicketList = new List<Ticket>();
            List<Ticket> _resultTicketListFinal = new List<Ticket>();
            List<TaskDto> _resultTicketDTOList = new List<TaskDto>();

            if (isAdmin)
            {
                if(filterIDs.Count == 0)
                {
                    foreach (var t in _resultTicketList)
                    {
                        _resultTicketDTOList.Add(new TaskDto(t));
                    }

                    return _resultTicketDTOList.AsQueryable();
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


            //foreach (string _filterID in filterIDs)
            //{
            //    _resultTicketList.AddRange(_ticketList.Where(t => t.TaskPriority == _filterID));
            //}

            //foreach (var t in _resultTicketList)
            //{
            //    if(_ )
            //}

            foreach (var t in _ticketList)
            {
                if (filterIDs.Contains(t.TaskStatus) && filterIDs.Contains(t.TaskPriority))
                {
                    _resultTicketListFinal.Add(t);
                }
            }

            foreach (var t in _resultTicketListFinal)
            {
                _resultTicketDTOList.Add(new TaskDto(t));
            }

            return _resultTicketDTOList.AsQueryable();

        }
    }
}