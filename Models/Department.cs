using System.ComponentModel.DataAnnotations;

namespace Wazifni.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        
    }
}