using Microsoft.AspNetCore.Mvc;

namespace Wazifni.Controllers;

public class DepartmentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}