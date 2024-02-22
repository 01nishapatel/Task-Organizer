using Database;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TaskOrganizer.ViewModel
{
    public class TaskUserViewModel
    {
       public Database.Task task { get; set; }
       // public Task task { get; set; }
        public User user { get; set; }
        public Attachment attach { get; set; }

        public virtual string UserName { get; set; }    
        public virtual SelectList UserList { get; set; }

        public virtual IFormFile? Attachments { get; set; }
        
        public Project project { get; set; }

        public ProjectAssign projectAssign { get; set; }

        public virtual SelectList ProjectList { get; set; }
    }
   
}
