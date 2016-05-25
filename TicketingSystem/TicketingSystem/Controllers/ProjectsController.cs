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

namespace TicketingSystem.Controllers
{
    //[Authorize]
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
        public async Task<IQueryable<Project>> GetProjects()
        {
            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            if(isAdmin)
            {
                return db.Projects.Include(p => p.Tasks);
            }
            return (from p in db.Projects.Include(p => p.AssignedUsers)
                    where p.AssignedUsers.Any(u => u.Id == User.Identity.Name)
                    select p).AsQueryable();
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
        public async Task<IHttpActionResult> PostProject(Project project)
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

            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectID }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            if (!isAdmin)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

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