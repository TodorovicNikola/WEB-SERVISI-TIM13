using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.DAL.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public String CommentContent { get; set; }
        public DateTime CommentCreated { get; set; }
        public DateTime CommentUpdated { get; set; }
    }
}
