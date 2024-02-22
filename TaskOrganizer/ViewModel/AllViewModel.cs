using Database;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TaskOrganizer.ViewModel
{
    public class AllViewModel
    {

        public Attachment Attachment { get; set; }

        public Comment Comment { get; set; }

        public Project Project { get; set; }

        public ProjectAssign ProjectAssign { get; set; }

        public IEnumerable<Database.Task> Task { get; set; }

        public IEnumerable<User> user { get; set; }

    }
}
