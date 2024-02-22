using Database;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TaskOrganizer.ViewModel
{
    public class ProjectAssignModel
    {
        public User user { get; set; }

        public Project project { get; set; }
        
        public ProjectAssign projectAssign { get;set; }

        public virtual string emailID { get; set; }

        public virtual SelectList emaillist { get; set; }

        public virtual IList<ProjectAssignModel> ProjectAssignModels { get; set; }
    }

    public class combinedModel
    {
        public ProjectAssignModel ProjectAssignModel { get; set; }

        public ProjectAssign ProjectAssign { get; set; }
    }
}
