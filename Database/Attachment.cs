using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Database
{
    public class Attachment
    {

        [Key]
        public int AttachmentID { get; set; }

        [Required]
        public AttachmentType? AttachmentType { get; set; }

        [Required]
        public string? Attachments { get; set; }        
        public string? FilePath { get; set; }
    }
    public enum AttachmentType
    {
        Photo, Video, Document, Other
    }
}
