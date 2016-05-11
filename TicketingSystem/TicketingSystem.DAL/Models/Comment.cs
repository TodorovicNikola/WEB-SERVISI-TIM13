using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingSystem.DAL.Models
{
    public class Comment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        [StringLength(1024), Required]
        public String CommentContent { get; set; }

        [Required]
        public DateTime CommentCreated { get; set; }

        public DateTime? CommentUpdated { get; set; }

        [Required]
        [ForeignKey("Task")]
        [Column(Order = 1)]
        public int TaskID { get; set; }

        [Required]
        [ForeignKey("Task")]
        [Column(Order = 2)]
        public int ProjectID { get; set; }

        [Required]
        [ForeignKey("UserWrote")]
        public int UserWroteID { get; set; }


        
        public virtual Task Task { get; set; }

        [ForeignKey("UserWroteID")]
        public virtual User UserWrote { get; set; }
    }
}
