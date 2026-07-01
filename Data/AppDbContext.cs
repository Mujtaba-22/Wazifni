using Microsoft.EntityFrameworkCore;   // call EntityFrameWork  // call EntityFrameWork 
using Wazifni.Models;
    
namespace Wazifni.Data
{
    public class AppDbContext:DbContext // EntityFramework
    {
            public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
            {
                
            }

            // create a table in the database here
            public DbSet<Freelancer> Freelancers { get; set; }
            public DbSet<Department> Departments { get; set; }
            public DbSet<Skill> Skills { get; set; }


       
    }

  
}


