using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazifni.Data;
using Wazifni.Models;

namespace Wazifni.Controllers;

public class SkillController : Controller
{
    private readonly AppDbContext _dbContext;

    public SkillController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Skill());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Skill skill)
    {
        if (!ModelState.IsValid)
        {
            return View(skill);
        }

        _dbContext.Skills.Add(skill);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var skill = await _dbContext.Skills.FindAsync(id);

        if (skill is null)
        {
            return NotFound();
        }

        return View(skill);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Skill skill)
    {
        if (!ModelState.IsValid)
        {
            return View(skill);
        }

        _dbContext.Skills.Update(skill);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("Dashboard", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var skill = await _dbContext.Skills
            .Include(item => item.Freelancers)
            .FirstOrDefaultAsync(item => item.SkillId == id);

        if (skill is null)
        {
            return NotFound();
        }

        if (skill.Freelancers.Any())
        {
            TempData["StatusMessage"] = "لا يمكن حذف المهارة لأنها مرتبطة بمستقلين.";
            return RedirectToAction("Dashboard", "Home");
        }

        _dbContext.Skills.Remove(skill);
        await _dbContext.SaveChangesAsync();

        TempData["StatusMessage"] = "تم حذف المهارة بنجاح.";
        return RedirectToAction("Dashboard", "Home");
    }
}