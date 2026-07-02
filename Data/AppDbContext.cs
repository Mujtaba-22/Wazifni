using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wazifni.Models;

namespace Wazifni.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Skill> Skills { get; set; }

        // لاحقًا إذا أنشأت جدول FreelancerSkill
        // public DbSet<FreelancerSkill> FreelancerSkills { get; set; }
    }
}