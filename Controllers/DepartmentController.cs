using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazifni.Data;
using Wazifni.Models;
using Wazifni.ViewModels;

namespace Wazifni.Controllers;

public class DepartmentController : Controller
{
    private readonly AppDbContext _dbContext;

    public DepartmentController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(int? id)
    {
        var departments = await _dbContext.Departments
            .AsNoTracking()
            .OrderBy(department => department.Name)
            .ToListAsync();

        var currentDepartment = id.HasValue
            ? departments.FirstOrDefault(department => department.DeptId == id.Value)
            : departments.FirstOrDefault();

        if (currentDepartment is null)
        {
            return NotFound();
        }

        var freelancers = await _dbContext.Freelancers
            .AsNoTracking()
            .Include(freelancer => freelancer.User)
            .Include(freelancer => freelancer.SkillEntity)
            .Where(freelancer => freelancer.DeptId == currentDepartment.DeptId)
            .OrderBy(freelancer => freelancer.FreelancerId)
            .ToListAsync();

        var model = new DepartmentIndexViewModel
        {
            DepartmentId = currentDepartment.DeptId,
            DepartmentName = currentDepartment.Name,
            Description = currentDepartment.Description ?? string.Empty,
            FreelancerCount = freelancers.Count,
            Departments = departments,
            FilterSkills = await _dbContext.Skills
                .AsNoTracking()
                .OrderBy(skill => skill.Name)
                .ToListAsync(),
            Freelancers = freelancers
                .Select(freelancer => new FreelancerCardViewModel
                {
                    FreelancerId = freelancer.FreelancerId,
                    FullName = freelancer.User != null ? freelancer.User.FullName : "مستقل",
                    RoleTitle = freelancer.SkillEntity?.Name ?? freelancer.Skill,
                    Summary = freelancer.Bio ?? string.Empty,
                    Skills = string.IsNullOrWhiteSpace(freelancer.SkillEntity?.Name ?? freelancer.Skill)
                        ? new List<string>()
                        : new List<string> { freelancer.SkillEntity?.Name ?? freelancer.Skill },
                    ProfileUrl = Url.Action("Profile", "Home", new { id = freelancer.FreelancerId }) ?? "/Home/Profile"
                })
                .ToList()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Department());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    {
        if (!ModelState.IsValid)
        {
            return View(department);
        }

        _dbContext.Departments.Add(department);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var department = await _dbContext.Departments.FindAsync(id);

        if (department is null)
        {
            return NotFound();
        }

        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Department department)
    {
        if (!ModelState.IsValid)
        {
            return View(department);
        }

        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Create));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var department = await _dbContext.Departments
            .Include(item => item.Freelancers)
            .FirstOrDefaultAsync(item => item.DeptId == id);

        if (department is null)
        {
            return NotFound();
        }

        if (department.Freelancers.Any())
        {
            TempData["StatusMessage"] = "لا يمكن حذف القسم لأنه مرتبط بمستقلين.";
            return RedirectToAction("Dashboard", "Home");
        }

        _dbContext.Departments.Remove(department);
        await _dbContext.SaveChangesAsync();

        TempData["StatusMessage"] = "تم حذف القسم بنجاح.";
        return RedirectToAction("Dashboard", "Home");
    }
}