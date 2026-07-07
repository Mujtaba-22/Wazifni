using Microsoft.AspNetCore.Identity;

namespace Wazifni.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public Freelancer? Freelancer { get; set; }
    }
}
