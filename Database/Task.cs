using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        public int AssigneeID { get; set; }

        [Required(ErrorMessage = "User Name Required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Select any TaskType")]
        public TaskType? TaskType { get; set; }

        [Required(ErrorMessage = "Project Name Required")]
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Task Title Required")]
        public string? TaskTitle { get; set; }

        public int AttachmentID { get; set; }
        
        public string? Description { get; set; }

        [Required(ErrorMessage = "Select any Priority")]
        public Priority? Priority { get; set; }


        [Required(ErrorMessage = "Select any Status")]
        public Status? Status { get; set; }

        [Required(ErrorMessage = "Start Date Required")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End Date Required")]
        public DateTime? EndDate { get; set; }
    }

    public enum TaskType
    {
        Bug, Enhancement, Design, QA
    }

    public enum Priority
    {
        Low, Medium, High
    }

    public enum Status
    {
        Pending, Inprogess, Completed, Reassigned
    }
}
