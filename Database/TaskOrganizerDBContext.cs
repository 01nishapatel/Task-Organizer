using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class TaskOrganizerDBContext : DbContext
    {
    
        public TaskOrganizerDBContext(DbContextOptions<TaskOrganizerDBContext> options) : base(options)
        {

        }
        
        public DbSet<User> User { get; set; }

        public DbSet<Task> Task { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<ProjectAssign> ProjectAssign { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Attachment> Attachment { get; set; }


    }
}
