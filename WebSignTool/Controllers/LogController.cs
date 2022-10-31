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
        private readonly IMongo mongo;
        private readonly ISql sql;

        public LogController(Microsoft.Extensions.Hosting.IHostEnvironment _env, IConfiguration _Configuration, IRedis _redis, IMongo _mongo, ISql _sql)
        {
            env = _env;
            Configuration = _Configuration;
            redis = _redis;
            mongo = _mongo;
            sql = _sql;
        }

        public async Task<IActionResult> LogAsync()
        {
            try
            {
                return Content(await Global.GetLog(Configuration, redis, mongo, sql));
            }
            catch (Exception ex)
            {
                return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
            }
        }
    }
}
