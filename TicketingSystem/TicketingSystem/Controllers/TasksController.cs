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
//Aleksa prvi commit
namespace TicketingSystem.Controllers
{
    //[Authorize(Users = "qwerty")]
    public class TasksController : ApiController
    {
        private TicketingSystemDBContext db = new TicketingSystemDBContext();

        // GET: api/Tasks
        public IQueryable<DAL.Models.Ticket> GetTasks()
        {
            return db.Tickets;
        }


        [Route("api/Projects/{projectId}/tasks/{taskId}")]
        [ResponseType(typeof(DAL.Models.Ticket))]
        public async Task<IHttpActionResult> GetTaskDetails(int taskId)
        {
            DAL.Models.Ticket task = await db.Tickets.Include(t => t.Comments).Include(t => t.Changes).SingleOrDefaultAsync(t => t.TicketID == taskId);
            //DAL.Models.Ticket task = await db.Tickets.Include(t => t.Changes).SingleOrDefaultAsync(t => t.TicketID == taskId);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(DAL.Models.Ticket))]
        public async Task<IHttpActionResult> GetTask(int id)
        {
            DAL.Models.Ticket task = await db.Tickets.Include(t => t.Comments).Include(t => t.Changes).SingleOrDefaultAsync(t => t.TicketID == id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTask(int id, DAL.Models.Ticket task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.TicketID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        [ResponseType(typeof(DAL.Models.Ticket))]
        public async Task<IHttpActionResult> PostTask(DAL.Models.Ticket task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickets.Add(task);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = task.TicketID }, task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(DAL.Models.Ticket))]
        public async Task<IHttpActionResult> DeleteTask(int id)
        {
            DAL.Models.Ticket task = await db.Tickets.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(task);
            await db.SaveChangesAsync();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tickets.Count(e => e.TicketID == id) > 0;
        }
    }
}