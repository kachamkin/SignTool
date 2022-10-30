using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using System.Data;

namespace WebSignTool.Controllers
{
    public class LogController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        private readonly IConfiguration Configuration;
        private readonly IRedis redis;

        public LogController(Microsoft.Extensions.Hosting.IHostEnvironment _env, IConfiguration _Configuration, IRedis _redis)
        {
            env = _env;
            Configuration = _Configuration;
            redis = _redis;
        }

        public async Task<IActionResult> LogAsync()
        {
            try
            {
                string dataBaseType = Configuration.GetSection("Options").GetValue<string>("DataBaseType");
                string ret = "";

                if (dataBaseType == "SQL") 
                {
                    foreach (LogEntry entry in new LogContext(Configuration).GetRecords())
                        ret += entry;

                    return Content(ret);
                }
                else if (dataBaseType == "Redis")
                {
                    foreach (DataRow entry in await redis.GetRecords())
                        ret += entry["value"] + "\r\n";

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
