using Microsoft.AspNetCore.Mvc;

namespace WebSignTool.Controllers
{
    public class LogController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
        public LogController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public async Task<IActionResult> LogAsync()
        {
            try
            {
                return Content(await System.IO.File.ReadAllTextAsync(Global.GetCertDir() + "\\log.txt"));
            }
            catch (Exception ex)
            {
                return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
            }
        }
    }
}
