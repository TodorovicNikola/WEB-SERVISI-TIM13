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

namespace TicketingSystem.Controllers
{
    public class UsersController : ApiController
    {
        private TicketingSystemDBContext db = new TicketingSystemDBContext();

        // GET: api/Users
        public IQueryable<TicketingSystemUser> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(TicketingSystemUser))]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            TicketingSystemUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(string id, TicketingSystemUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

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
        public async Task<IHttpActionResult> PostUser(TicketingSystemUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(TicketingSystemUser))]
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

        // GET: api/Projects/5/Users
        [Route("api/projects/{projectId}/users")]
        [ResponseType(typeof(TicketingSystemUser))]
        public IHttpActionResult GetProjectUsers(int projectId)
        {
            var users = from u in db.Users.Include(u => u.AssignedProjects)
                        where u.AssignedProjects.Any(p => p.ProjectID == projectId)
                        select u;

            return Ok(users.AsQueryable());
        }

        // GET: api/Projects/5/Users
        [Route("api/projects/{projectId}/users/{userId}")]
        [ResponseType(typeof(TicketingSystemUser))]
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