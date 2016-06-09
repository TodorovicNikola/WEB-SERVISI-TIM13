using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
using TicketingSystem.DTOs;

namespace TicketingSystem.Controllers
{
    [Authorize]
    public class UsersController : ApiController
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

        // GET: api/Users
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetUsers()
        {
            var users = db.Users;
            var retUsers = new LinkedList<UserDTO>();

            foreach(var u in users)
            {
                retUsers.AddLast(new UserDTO { FirstName = u.FirstName, LastName = u.LastName, UserName = u.Id, Email = u.Email });
            }

            return Ok(retUsers);
        }

        // GET: api/Users/5
        [ResponseType(typeof(TicketingSystemUser))]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            bool isAdmin = UserManager.IsInRole(User.Identity.Name, "Admin");
            TicketingSystemUser user = null;

            if (isAdmin || User.Identity.Name == id)
            {
                user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            } else
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }
            if (user == null)
            {
                return NotFound();
            }

            var retUser = new UserDTO { FirstName = user.FirstName, LastName = user.LastName, UserName = user.Id, Email = user.Email };

            return Ok(retUser);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> PutUser(string id, UserDTO usr)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usr.UserName)
            {
                return BadRequest();
            }

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

            user.Email = usr.Email;
            user.PasswordHash = TicketingSystemUser.HashPassword(usr.Password);
            user.FirstName = usr.FirstName;
            user.LastName = usr.LastName;

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(TicketingSystemUser))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> PostUser(UserDTO user)
        {
            if (!ModelState.IsValid || user.Password.Length < 6)
            {
                return BadRequest(ModelState);
            }

            var usr = new TicketingSystemUser()
            {      
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = true,
                Id = user.UserName,
                UserType = TicketingSystemUser.UserTypes.USER,
                PasswordHash = TicketingSystemUser.HashPassword(user.Password)
            };

            IdentityResult result = UserManager.Create(usr);
            await UserManager.AddToRoleAsync(usr.Id, "User");

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(TicketingSystemUser))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            TicketingSystemUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        // GET: api/Users/5/Finished
        [Route("api/users/{userId}/finished")]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetUserReport(string userId)
        {
            TicketingSystemUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            var tasks = (from t in db.Tickets
                         where t.UserAssignedID == userId && t.TaskStatus == "Done"
                         orderby t.TaskFrom
                         select t).AsQueryable();

            List<TaskDto> sortedTasks = new List<TaskDto>();

            foreach (var t in tasks)
            {
                sortedTasks.Add(new TaskDto(t));
            }

            return Ok(sortedTasks);
        }

        // GET: api/Projects/5/Users
        [Route("api/projects/{projectId}/users")]
        [ResponseType(typeof(TicketingSystemUser))]
       // [Authorize(Roles = "Admin")]
        public IHttpActionResult GetProjectUsers(int projectId)
        {
            var users = (from u in db.Users.Include(u => u.AssignedProjects)
                         where u.AssignedProjects.Any(p => p.ProjectID == projectId)
                         select u).AsQueryable();

            var retUsers = new LinkedList<UserDTO>();

            foreach (var u in users)
            {
                retUsers.AddLast(new UserDTO { FirstName = u.FirstName, LastName = u.LastName, UserName = u.Id, Email = u.Email });
            }

            return Ok(retUsers);
        }

        // POST: api/Projects/5/Users
        [Route("api/projects/{projectId}/users/{userId}")]
        [ResponseType(typeof(TicketingSystemUser))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> PostProjectUser(int projectId, string userId)
        {
            Project project = db.Projects.Include(path => path.AssignedUsers).FirstOrDefault(p => p.ProjectID == projectId);
            var user = db.Users.Find(userId);

            if (project != null && user != null)
            {
                if (project.AssignedUsers.Contains(user))
                {
                    return BadRequest("Already assigned.");
                }
                project.AssignedUsers.Add(user);

                db.Entry(project).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return NotFound();
        }

        // Delete: api/Projects/5/Users/5
        [Route("api/projects/{projectId}/users/{userId}"), HttpDelete]
        [ResponseType(typeof(TicketingSystemUser))]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> DeleteProjectUser(int projectId, string userId)
        {
            Project project = db.Projects.Include(path => path.AssignedUsers).FirstOrDefault(p => p.ProjectID == projectId);
            var user = db.Users.Find(userId);

            if (project != null && user != null)
            {
                if (!project.AssignedUsers.Contains(user))
                {
                    return BadRequest("Already removed.");
                }
                project.AssignedUsers.Remove(user);

                db.Entry(project).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}