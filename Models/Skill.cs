using System.ComponentModel.DataAnnotations;

namespace Wazifni.Models
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        
    }
}