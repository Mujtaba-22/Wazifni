using Microsoft.EntityFrameworkCore;
using Wazifni.Models;

namespace Wazifni.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (!await dbContext.Departments.AnyAsync())
        {
            dbContext.Departments.AddRange(
                new Department { Name = "البرمجة والتطوير", Icon = "bi-laptop", Description = "مطورين ويب، تطبيقات، وقواعد بيانات" },
                new Department { Name = "القانون", Icon = "bi-bank", Description = "استشارات قانونية وصياغة عقود" },
                new Department { Name = "التعليم", Icon = "bi-book", Description = "مدرسين خصوصيين وشروحات أكاديمية" },
                new Department { Name = "التدريب", Icon = "bi-mic", Description = "مدربين لتطوير المهارات الشخصية" }
            );
        }

        

        if (!await dbContext.Skills.AnyAsync())
        {
            dbContext.Skills.AddRange(
                new Skill { Name = "ASP.NET" },
                new Skill { Name = "C#" },
                new Skill { Name = "SQL Server" },
                new Skill { Name = "Bootstrap" },
                new Skill { Name = "JavaScript" },
                new Skill { Name = "React" },
                new Skill { Name = "Entity Framework" },
                new Skill { Name = "HTML/CSS" }
            );
        }

        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync();
        }
    }
}