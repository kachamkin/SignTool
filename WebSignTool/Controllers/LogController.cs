using Microsoft.AspNetCore.Mvc;

namespace WebSignTool.Controllers
{
    public class LogController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        private readonly IConfiguration Configuration;
        public LogController(Microsoft.Extensions.Hosting.IHostEnvironment _env, IConfiguration _Configuration)
        {
            env = _env;
            Configuration = _Configuration;
        }

        public async Task<IActionResult> LogAsync()
        {
            try
            {
                if (env.IsDevelopment())
                {
                    string ret = "";
                    foreach (LogEntry entry in new LogContext(Configuration).GetRecords())
                        ret += entry;

                    return Content(ret);
                }
                else
                    return Content(await System.IO.File.ReadAllTextAsync(Global.GetCertDir() + "\\log.txt"));
            }
            catch (Exception ex)
            {
                return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
            }
        }
    }
}
