using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wazifni.Models
{
    public class Freelancer
    {
        [Key]
        public int FreelancerId { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Skill { get; set; } = string.Empty;

        public int? SkillId { get; set; }

        [ForeignKey(nameof(SkillId))]
        public Skill? SkillEntity { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        public string? CVFile { get; set; }

        [NotMapped]
        public IFormFile? CVUpload { get; set; }
        public int? YearsExperience { get; set; }

        [Url]
        public string? LinkedInUrl { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        public int? DeptId { get; set; }

        // Navigation
        [ForeignKey(nameof(DeptId))]
        public Department? Department { get; set; }

        public ApplicationUser? User { get; set; }

    }
}