using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        private readonly IConfiguration Configuration;
        private readonly IHostEnvironment env;

        public LogFilter(IConfiguration _config, IHostEnvironment _env)
        {
            Configuration = _config;
            env = _env;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IPAddress? ip = context.HttpContext.Connection.RemoteIpAddress;

            try
            {
                if (env.IsDevelopment())
                {
                    new LogContext(Configuration).AddRecord(DateTime.Now, ip + GetHostName(ip), context.HttpContext.Request.Path);
                }
                else
                {
                    string LogName = Global.GetCertDir() + "\\" + "log.txt";

                    FileInfo fi = new(LogName);
                    using FileStream fs = new(LogName, fi.Exists && fi.Length > Configuration.GetSection("Options").GetValue<long>("MaxLogSize") ? FileMode.Create : FileMode.OpenOrCreate);
                    fs.Seek(0, SeekOrigin.End);

                    using StreamWriter bw = new(fs);
                    bw.Write(DateTime.Now + "   ");

                    bw.Write(ip + GetHostName(ip) + "  ");
                    bw.Write(context.HttpContext.Request.Path + "\r\n");

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
