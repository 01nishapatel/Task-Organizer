using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public class ProjectAssign
    {
        [Key]
        public int ProjectAssignID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        public UserType? UserType { get; set; }
    }

    public enum UserType
    {
        ProjectManager,TeamLeader , Developer
    }
}
