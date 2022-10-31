using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Net;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        private readonly IConfiguration Configuration;
        private readonly ITelegram telegram;
        private readonly IRedis redis;
        private readonly IMongo mongo;
        private readonly ISql sql;

        public LogFilter(IConfiguration _config, ITelegram _telegram, IRedis _redis, IMongo _mongo, ISql _sql)
        {
            Configuration = _config;
            telegram = _telegram;
            redis = _redis;
            mongo = _mongo;
            sql = _sql;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IPAddress? ip = context.HttpContext.Connection.RemoteIpAddress;

            try
            {

                string hostName = GetHostName(ip);

                telegram.SendMessage("Host: " + ip + hostName + "\nPath: " + context.HttpContext.Request.Path + "\nClient: " + context.HttpContext.Request.Headers["User-Agent"]);

                string dataBaseType = Configuration.GetSection("Options").GetValue<string>("DataBaseType");

                if (dataBaseType == "SQL")
                {
                    sql.AddRecord(DateTime.Now, ip + GetHostName(ip), context.HttpContext.Request.Path + ", client: " + context.HttpContext.Request.Headers["User-Agent"]);
                }
                else if (dataBaseType == "Redis")
                    await redis.AddRecord(DateTime.Now, ip + hostName, context.HttpContext.Request.Path + ", client: " + context.HttpContext.Request.Headers["User-Agent"]);
                else if (dataBaseType == "Mongo")
                    await mongo.AddRecord(DateTime.Now, ip + hostName, context.HttpContext.Request.Path + ", client: " + context.HttpContext.Request.Headers["User-Agent"]);
                else
                {
                    string LogName = Global.GetCertDir() + "\\" + "log.txt";

                    FileInfo fi = new(LogName);
                    using FileStream fs = new(LogName, fi.Exists && fi.Length > Configuration.GetSection("Options").GetValue<long>("MaxLogSize") ? FileMode.Create : FileMode.OpenOrCreate);
                    fs.Seek(0, SeekOrigin.End);

                    using StreamWriter bw = new(fs);
                    bw.Write(DateTime.Now + "   ");

                    bw.Write(ip + hostName + "  ");
                    bw.Write(context.HttpContext.Request.Path + ", client: " + context.HttpContext.Request.Headers["User-Agent"] + "\r\n");

                    bw.Close();
                    fs.Close();
                }
            }
            catch
            {
            }

            await next();
        }

        public string GetHostName(IPAddress? ip)
        {
            string HostName;

            try
            {
                HostName = ip == null ? "" : " (" + Dns.GetHostEntry(ip).HostName + ")";
            }
            catch (Exception ex)
            {
                HostName = " (" + ex.Message + ")";
            }

            return HostName;
        }
    }
}
