using Database;
namespace TaskOrganizer.ViewModel
{
    public class ProjectViewModel
    {
        public Project project { get; set; }

        public IEnumerable<Project> Projects { get; set; }
    }
}
