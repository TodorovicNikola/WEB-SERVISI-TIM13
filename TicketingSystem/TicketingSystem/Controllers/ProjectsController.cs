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

namespace TicketingSystem.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        private TicketingSystemDBContext db = new TicketingSystemDBContext();

        // GET: api/Projects
        public IQueryable<Project> GetProjects()
        {
            return (from p in db.Projects.Include(p => p.AssignedUsers)
                    where p.AssignedUsers.Any(u => u.Email == User.Identity.Name)
                    select p).AsQueryable();
        }


        private static readonly Expression<Func<DAL.Models.Ticket, TaskDto>> AsTaskDto =
            x => new TaskDto
            {
                TaskName = x.TaskName,
                TaskFrom = x.TaskFrom,
                TaskUntil = x.TaskUntil,
                TaskPriority = x.TaskPriority,
                TaskDescription = x.TaskDescription,
                TaskStatus = x.TaskStatus,
                UserAssigned = x.UserAssigned.UserName,
                UserCreated = x.UserCreated.UserName,
                TaskId = x.TicketID





            };

        [Route("api/Projects/{projectId}/tasks")]
        public IQueryable<DTOs.TaskDto> GetTasksOfProject(int projectId)
        {
            var data = (from p in db.Projects.Include(p => p.AssignedUsers)
                        where p.AssignedUsers.Any(u => u.Email == User.Identity.Name) && p.ProjectID == projectId
                        select p).Count();
            
            if (data == 0)
            {
                return null;
            }

            return db.Tickets.Include(b => b.Project)
                .Where(b => b.ProjectID == projectId)
                .Select(AsTaskDto);
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
            // TODO : role check

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
            // TODO : role check

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
            // TODO : admin check

            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return Ok(project);
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