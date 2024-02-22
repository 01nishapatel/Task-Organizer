using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public class Comment
    {

        [Key]
        public int CommentID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int TaskID { get; set; }

        [Required]
        public string? comments { get; set; }

        public int AttachmentID { get; set; }

    }
}
