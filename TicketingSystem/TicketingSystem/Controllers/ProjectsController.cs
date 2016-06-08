using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TicketingSystem.DAL;
using TicketingSystem.DAL.Models;
using System.Linq.Expressions;
using TicketingSystem.DTOs;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace TicketingSystem.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
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

        // GET: api/Projects
        public IHttpActionResult GetProjects()
        {
            bool isAdmin = UserManager.IsInRole(User.Identity.Name, "Admin");

            if (isAdmin)
            {
                return Ok(db.Projects.Include(p => p.Tasks));
            }
            var projects = (from p in db.Projects.Include(p => p.AssignedUsers).Include(t => t.Tasks)
                            where p.AssignedUsers.Any(u => u.Id == User.Identity.Name)
                            select p).AsQueryable();

            foreach (var p in projects)
            {
                p.AssignedUsers = null;
            }

            return Ok(projects);

            //return Ok(db.Projects);
        }


        

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await db.Projects.Include(p => p.AssignedUsers).Include(p => p.Tasks).SingleOrDefaultAsync(p => p.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            project.AssignedUsers = null;

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProject(int id, Project project)
        {
            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            if (!isAdmin)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectID)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectID }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return Ok(project);
        }
        
        //POST: api/Projects/Filter
        [Route("GetFilter")]
        public async Task<IQueryable<Project>> GetFilter(string[] filterIDs)
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

        // GET: api/Projects/5/Assigned
        [Route("api/projects/{projectId}/assignedPercent")]
        [ResponseType(typeof(ProjectTicketsDTO))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAssignedReport(int projectId)
        {
            var users = (from u in db.Users.Include(u => u.AssignedProjects)
                         where u.AssignedProjects.Any(p => p.ProjectID == projectId)
                         select u).AsQueryable();

            var project = db.Projects.Find(projectId);
            var tasks = (from t in db.Tickets
                         where t.ProjectID == projectId
                         select t).AsQueryable();

            int unassigned = 0;

            Dictionary<String, int> assigned = new Dictionary<String, int>();
            Dictionary<String, TicketingSystemUser> usersDict = new Dictionary<String, TicketingSystemUser>();

            foreach (var u in users)
            {
                assigned.Add(u.UserName, 0);
                usersDict.Add(u.UserName, u);
            }

            foreach (var t in tasks)
            {
                if (t.UserAssigned != null)
                {
                    if (assigned.ContainsKey(t.UserAssigned.UserName))
                    {
                        assigned[t.UserAssigned.UserName] += 1;
                    }
                    else
                    {
                        usersDict.Add(t.UserAssigned.UserName, t.UserAssigned);
                        assigned.Add(t.UserAssigned.UserName, 0);
                    }
                }
                else
                {
                    unassigned++;
                }
            }

            var ret = new ProjectTicketsDTO();
            ret.Project = new ProjectDTO(project);

            ret.Users = new LinkedList<Tuple<UserDTO, Double>>();
            foreach (var u in assigned.Keys)
            {
                var tpl = new Tuple<UserDTO, Double>(new UserDTO(usersDict[u]), Math.Round(assigned[u] * 1.0 / tasks.Count(), 4));
                ret.Users.Add(tpl);
            }

            ret.Unassigned = Math.Round(unassigned * 1.0 / tasks.Count(), 4);

            return Ok(ret);
        }

        // GET: api/Projects/5/Finished
        [Route("api/projects/{projectId}/finishedPercent")]
        [ResponseType(typeof(ProjectTicketsDTO))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetFinishedReport(int projectId)
        {
            var users = (from u in db.Users.Include(u => u.AssignedProjects)
                         where u.AssignedProjects.Any(p => p.ProjectID == projectId)
                         select u).AsQueryable();

            var project = db.Projects.Find(projectId);
            var tasks = (from t in db.Tickets
                         where t.ProjectID == projectId && t.TaskStatus == "Done"
                         select t).AsQueryable();

            int unassigned = 0;

            Dictionary<String, int> assigned = new Dictionary<String, int>();
            Dictionary<String, TicketingSystemUser> usersDict = new Dictionary<String, TicketingSystemUser>();

            foreach (var u in users)
            {
                assigned.Add(u.UserName, 0);
                usersDict.Add(u.UserName, u);
            }

            foreach (var t in tasks)
            {
                if (t.UserAssigned != null)
                {
                    if (assigned.ContainsKey(t.UserAssigned.UserName))
                    {
                        assigned[t.UserAssigned.UserName] += 1;
                    }
                    else
                    {
                        usersDict.Add(t.UserAssigned.UserName, t.UserAssigned);
                        assigned.Add(t.UserAssigned.UserName, 0);
                    }
                }
                else
                {
                    unassigned++;
                }
            }

            var ret = new ProjectTicketsDTO();
            ret.Project = new ProjectDTO(project);

            ret.Users = new LinkedList<Tuple<UserDTO, Double>>();
            foreach (var u in assigned.Keys)
            {
                var tpl = new Tuple<UserDTO, Double>(new UserDTO(usersDict[u]), Math.Round(assigned[u] * 1.0 / tasks.Count(), 4));
                ret.Users.Add(tpl);
            }

            ret.Unassigned = Math.Round(unassigned * 1.0 / tasks.Count(), 4);

            return Ok(ret);
        }
         
        // GET: api/Projects/5/Finished
        [Route("api/projects/{projectId}/created")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetCreatedTickets(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var tasks = (from t in db.Tickets
                         where t.ProjectID == projectId
                         orderby t.TaskCreated
                         select t).AsQueryable();

            List<TaskDto> sortedTasks = new List<TaskDto>();

            foreach (var t in tasks)
            {
                sortedTasks.Add(new TaskDto(t));
            }

            return Ok(sortedTasks);
        }

        [Route("api/projects/{projectId}/finished")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetFinishedTickets(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var tasks = (from t in db.Tickets
                         where t.ProjectID == projectId && t.TaskStatus == "Done"
                         orderby t.TaskCreated
                         select t).AsQueryable();

            List<TaskDto> sortedTasks = new List<TaskDto>();

            foreach (var t in tasks)
            {
                sortedTasks.Add(new TaskDto(t));
            }

            return Ok(sortedTasks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectID == id) > 0;
        }
    }
}