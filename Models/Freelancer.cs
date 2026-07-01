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
        public string Phone { get; set; } 
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public string Skill { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        public string? CVFile { get; set; }

        [NotMapped]
        public IFormFile? CVUpload { get; set; }

        [Url]
        public string? LinkedInUrl { get; set; }

        [StringLength(100)]
        public string? City { get; set; }
        // Navigation
        [ForeignKey("DeptId")]
        public Department? Department { get; set; }

    }
}