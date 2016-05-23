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

namespace TicketingSystem.Controllers
{
    public class CommentsController : ApiController
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

        // GET: api/Comments
        public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public async Task<IHttpActionResult> GetComment(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.CommentID)
            {
                return BadRequest();
            }

            var data = (from p in db.Projects.Include(p => p.AssignedUsers)
                        where p.AssignedUsers.Any(u => u.Id == User.Identity.Name) && p.ProjectID == comment.ProjectID
                        select p).Count();

            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            if (data == 0 && !isAdmin)
            {
                return BadRequest();
            }

            // TODO : CommentDto => Comment

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        [ResponseType(typeof(Comment))]
        public async Task<IHttpActionResult> PostComment(Comment comment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var data = (from p in db.Projects.Include(p => p.AssignedUsers)
                            where p.AssignedUsers.Any(u => u.Id == User.Identity.Name) && p.ProjectID == comment.ProjectID
                            select p).Count();

                bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

                if (data == 0 && !isAdmin)
                {
                    return BadRequest();
                }



                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = comment.CommentID }, comment);
            }

            catch (Exception e)
            {
                Console.WriteLine(e); // or log to file, etc.
                throw; // re-throw the exception if you want it to continue up the stack
            }
            }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public async Task<IHttpActionResult> DeleteComment(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            await db.SaveChangesAsync();

            return Ok(comment);
        }

        [Route("api/Projects/{projectId}/tasks/{taskId}/comments")]
        public async Task<IQueryable<Comment>> GetTasksOfProject(int projectId, int taskId)
        {
            var data = (from p in db.Projects.Include(p => p.AssignedUsers)
                        where p.AssignedUsers.Any(u => u.Id == User.Identity.Name) && p.ProjectID == projectId
                        select p).Count();

            bool isAdmin = await UserManager.IsInRoleAsync(User.Identity.Name, "Admin");

            if (data == 0 && !isAdmin)
            {
                return null;
            }

            return db.Comments.Include(c => c.Task).Where(c => c.Task.TicketID == taskId && c.Task.ProjectID == projectId).AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.CommentID == id) > 0;
        }
    }
}