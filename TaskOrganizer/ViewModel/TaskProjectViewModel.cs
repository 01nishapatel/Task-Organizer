using Database;
namespace TaskOrganizer.ViewModel
{
    public class TaskProjectViewModel
    {
        public Project project { get; set; }

        public Database.Task tasks { get; set; }
    }
}
