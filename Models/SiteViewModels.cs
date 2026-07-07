using System.ComponentModel.DataAnnotations;
using Wazifni.Models;

namespace Wazifni.ViewModels;

public class HomeIndexViewModel
{
    public string HeroBadge { get; set; } = "منصة العمل الحر";
    public string HeroTitle { get; set; } = "أنجز أعمالك مع أفضل المستقلين المحترفين";
    public string HeroDescription { get; set; } = "وظفني تربط أصحاب الأعمال بالمستقلين في البرمجة، القانون، التعليم والتدريب.";

    public List<FeatureItemViewModel> Features { get; set; } = new();
}

public class FeatureItemViewModel
{
    public string Icon { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class FreelancerProfileViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string WhatsAppUrl { get; set; } = string.Empty;
    public string LinkedInUrl { get; set; } = string.Empty;
    public string CvUrl { get; set; } = string.Empty;
    public int YearsExperience { get; set; }

    public List<string> Skills { get; set; } = new();
}

public class DepartmentIndexViewModel
{
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int FreelancerCount { get; set; }

    public List<Department> Departments { get; set; } = new();

    public List<Skill> FilterSkills { get; set; } = new();
    public List<FreelancerCardViewModel> Freelancers { get; set; } = new();
}

public class FreelancerCardViewModel
{
    public int FreelancerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string RoleTitle { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
    public string ProfileUrl { get; set; } = string.Empty;
}

public class DashboardViewModel
{
    public List<Department> Departments { get; set; } = new();
    public List<Skill> Skills { get; set; } = new();
}