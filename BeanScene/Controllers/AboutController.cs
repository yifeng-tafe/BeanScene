using Microsoft.AspNetCore.Mvc;

namespace BeanScene.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
