using Microsoft.AspNetCore.Mvc;

namespace CloudStorage.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}