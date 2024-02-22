using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Project Name is required..")]
        public string? ProjectTitle { get; set; }

        public string? Key { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int UserID { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Start Date is required..")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required..")]
        public DateTime? EndDate { get; set; }



    }
}
