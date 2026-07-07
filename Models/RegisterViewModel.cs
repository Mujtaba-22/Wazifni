using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Wazifni.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public int? DeptId { get; set; }

        [Required]
        public int? SkillId { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [Url]
        public string? LinkedInUrl { get; set; }

        public string? CvUrl { get; set; }
        public int? YearsExperience { get; set; }

        public IEnumerable<SelectListItem> DepartmentOptions { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> SkillOptions { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}