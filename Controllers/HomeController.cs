using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazifni.Data;
using Wazifni.Models;
using Wazifni.ViewModels;

namespace Wazifni.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var model = new HomeIndexViewModel
        {
            Features = new List<FeatureItemViewModel>
            {
                new() { Icon = "bi-check-circle-fill", Title = "واجهة عربية واضحة", Description = "تجربة تصفح مباشرة وسهلة للمستخدم." },
                new() { Icon = "bi-check-circle-fill", Title = "صفحات جاهزة", Description = "عرض المستقلين والأقسام بشكل منظم." },
                new() { Icon = "bi-check-circle-fill", Title = "Bootstrap متجاوب", Description = "تناسق كامل مع الأجهزة المختلفة والوضع الليلي." }
            }
        };

        return View(model);
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new DashboardViewModel
        {
            Departments = await _dbContext.Departments
                .AsNoTracking()
                .OrderBy(department => department.Name)
                .ToListAsync(),
            Skills = await _dbContext.Skills
                .AsNoTracking()
                .OrderBy(skill => skill.Name)
                .ToListAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> Profile(int? id)
    {
        var freelancer = id.HasValue
            ? await _dbContext.Freelancers
                .AsNoTracking()
                .Include(item => item.Department)
                .Include(item => item.SkillEntity)
                .Include(item => item.User)
                .FirstOrDefaultAsync(item => item.FreelancerId == id.Value)
            : await _dbContext.Freelancers
                .AsNoTracking()
                .Include(item => item.Department)
                .Include(item => item.SkillEntity)
                .Include(item => item.User)
                .FirstOrDefaultAsync();

        if (freelancer is null)
        {
            return NotFound();
        }

        var model = new FreelancerProfileViewModel
        {
            FullName = freelancer.User?.FullName ?? "مستقل",
            Title = freelancer.SkillEntity?.Name ?? freelancer.Skill,
            DepartmentName = freelancer.Department?.Name ?? "بدون قسم",
            Bio = freelancer.Bio ?? string.Empty,
            Phone = freelancer.Phone,
            City = freelancer.City ?? string.Empty,
            Email = freelancer.User?.Email ?? string.Empty,
            WhatsAppUrl = string.IsNullOrWhiteSpace(freelancer.Phone)
                ? "#"
                : $"https://wa.me/{new string(freelancer.Phone.Where(char.IsDigit).ToArray())}",
            LinkedInUrl = freelancer.LinkedInUrl ?? string.Empty,
            CvUrl = freelancer.CVFile ?? string.Empty,
            YearsExperience = freelancer.YearsExperience ?? 0,
            Skills = string.IsNullOrWhiteSpace(freelancer.SkillEntity?.Name ?? freelancer.Skill)
                ? new List<string>()
                : new List<string> { freelancer.SkillEntity?.Name ?? freelancer.Skill }
        };

        return View(model);
    }
 // GET: /Home/MyProfile
    [HttpGet]
    public async Task<IActionResult> MyProfile(int? id)
    {
        var freelancer = id.HasValue
            ? await _dbContext.Freelancers
                .AsNoTracking()
                .Include(item => item.Department)
                .Include(item => item.SkillEntity)
                .Include(item => item.User)
                .FirstOrDefaultAsync(item => item.FreelancerId == id.Value)
            : await _dbContext.Freelancers
                .AsNoTracking()
                .Include(item => item.Department)
                .Include(item => item.SkillEntity)
                .Include(item => item.User)
                .FirstOrDefaultAsync();

        if (freelancer is null)
        {
            return NotFound();
        }

        var model = new FreelancerProfileViewModel
        {

            FullName = freelancer.User?.FullName ?? "مستقل",
            Title = freelancer.SkillEntity?.Name ?? freelancer.Skill,
            DepartmentName = freelancer.Department?.Name ?? "بدون قسم",
            Bio = freelancer.Bio ?? string.Empty,
            Phone = freelancer.Phone,
            City = freelancer.City ?? string.Empty,
            Email = freelancer.User?.Email ?? string.Empty,
            CvUrl = freelancer.CVFile ?? string.Empty,
            YearsExperience = freelancer.YearsExperience ?? 0,
            Skills = string.IsNullOrWhiteSpace(freelancer.SkillEntity?.Name ?? freelancer.Skill)
                ? new List<string>()
                : new List<string> { freelancer.SkillEntity?.Name ?? freelancer.Skill }
        };

        return View(model);
    }

    // POST: /Home/MyProfile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MyProfile(FreelancerProfileViewModel model, IFormFile? AvatarFile, IFormFile? CvFile, List<string>? Skills, int? id)
    {
        var freelancer = await _dbContext.Freelancers
            .Include(item => item.Department)
            .Include(item => item.SkillEntity)
            .Include(item => item.User)
            .FirstOrDefaultAsync(item => item.FreelancerId == id.Value);

        if (freelancer is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.DepartmentName = freelancer.Department?.Name ?? "بدون قسم";
            model.CvUrl = freelancer.CVFile ?? string.Empty;
            model.YearsExperience = freelancer.YearsExperience ?? 0;
            return View(model);
        }

        if (freelancer.User != null)
        {
            freelancer.User.FullName = model.FullName;
            freelancer.User.Email = model.Email;
        }

        freelancer.Bio = model.Bio;
        freelancer.Phone = model.Phone;
        freelancer.City = model.City;
        freelancer.YearsExperience = model.YearsExperience;
        freelancer.Skill = Skills is { Count: > 0 } ? string.Join(", ", Skills) : freelancer.Skill;

        if (AvatarFile is { Length: > 0 })
        {
            var avatarsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(avatarsFolder);

            var avatarFileName = $"{Guid.NewGuid()}{Path.GetExtension(AvatarFile.FileName)}";
            var avatarFullPath = Path.Combine(avatarsFolder, avatarFileName);

            await using (var stream = new FileStream(avatarFullPath, FileMode.Create))
            {
                await AvatarFile.CopyToAsync(stream);
            }

        }

        if (CvFile is { Length: > 0 })
        {
            var cvFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "cv");
            Directory.CreateDirectory(cvFolder);

            var cvFileName = $"{Guid.NewGuid()}{Path.GetExtension(CvFile.FileName)}";
            var cvFullPath = Path.Combine(cvFolder, cvFileName);

            await using (var stream = new FileStream(cvFullPath, FileMode.Create))
            {
                await CvFile.CopyToAsync(stream);
            }

            freelancer.CVFile = $"/uploads/cv/{cvFileName}";
        }

        await _dbContext.SaveChangesAsync();

        TempData["SuccessMessage"] = "تم تحديث ملفك الشخصي بنجاح.";
        return RedirectToAction(nameof(MyProfile), new { id = freelancer.FreelancerId });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}