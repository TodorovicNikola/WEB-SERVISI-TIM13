using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TicketingSystem.DAL.Models
{
    public class Comment
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }

        [StringLength(1024, ErrorMessage = "'Comment Content' must not be longer than 1024 characters!"), Required(ErrorMessage = "'Comment Content' must not be empty!")]
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
        public string UserWroteID { get; set; }


        
        public virtual Ticket Task { get; set; }

        [JsonIgnore]
        [ForeignKey("UserWroteID")]
        public virtual TicketingSystemUser UserWrote { get; set; }
    }
}
