using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult NotFound404()
    {
        return View("NotFound");
    }

    [Route("Error/403")]
    public IActionResult Forbidden403()
    {
        return View("AccessDenied"); 
    }
}
