using Microsoft.AspNetCore.Mvc;

namespace WebSignTool.Controllers
{
    public class CreateguidController : Controller
    {
        public IActionResult Createguid()
        {
            return Content(Guid.NewGuid().ToString());
        }
    }
}
