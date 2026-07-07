using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wazifni.Models;
using Wazifni.Data;
using Wazifni.ViewModels;

namespace Wazifni.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            AppDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ===========================
        // Register
        // ===========================

        private async Task PopulateRegisterOptionsAsync(RegisterViewModel model)
        {
            model.DepartmentOptions = await _dbContext.Departments
                .AsNoTracking()
                .OrderBy(department => department.Name)
                .Select(department => new SelectListItem
                {
                    Value = department.DeptId.ToString(),
                    Text = department.Name
                })
                .ToListAsync();

            model.SkillOptions = await _dbContext.Skills
                .AsNoTracking()
                .OrderBy(skill => skill.Name)
                .Select(skill => new SelectListItem
                {
                    Value = skill.SkillId.ToString(),
                    Text = skill.Name
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel();
            await PopulateRegisterOptionsAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateRegisterOptionsAsync(model);
                return View(model);
            }

            if (model.DeptId is null || model.SkillId is null)
            {
                ModelState.AddModelError(string.Empty, "يرجى اختيار القسم والمهارة.");
                await PopulateRegisterOptionsAsync(model);
                return View(model);
            }

            var selectedDepartment = await _dbContext.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(department => department.DeptId == model.DeptId.Value);

            if (selectedDepartment is null)
            {
                ModelState.AddModelError(nameof(model.DeptId), "القسم المختار غير موجود.");
                await PopulateRegisterOptionsAsync(model);
                return View(model);
            }

            var selectedSkill = await _dbContext.Skills
                .AsNoTracking()
                .FirstOrDefaultAsync(skill => skill.SkillId == model.SkillId.Value);

            if (selectedSkill is null)
            {
                ModelState.AddModelError(nameof(model.SkillId), "المهارة المختارة غير موجودة.");
                await PopulateRegisterOptionsAsync(model);
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var freelancer = new Freelancer
                {
                    UserId = user.Id,
                    Phone = model.Phone,
                    DeptId = selectedDepartment.DeptId,
                    SkillId = selectedSkill.SkillId,
                    Skill = selectedSkill.Name,
                    Bio = model.Bio,
                    City = model.City,
                    LinkedInUrl = model.LinkedInUrl,
                    CVFile = model.CvUrl,
                    YearsExperience = model.YearsExperience
                };

                _dbContext.Freelancers.Add(freelancer);
                await _dbContext.SaveChangesAsync();

                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            await PopulateRegisterOptionsAsync(model);
            return View(model);
        }

        // ===========================
        // Login
        // ===========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة.");

            return View(model);
        }

        // ===========================
        // Logout
        // ===========================

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}