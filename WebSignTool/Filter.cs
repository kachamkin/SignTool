using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        private readonly IConfiguration Configuration;
        public LogFilter(IConfiguration _config)
        {
            Configuration = _config;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            try
            {
                string LogName = Global.GetCertDir() + "\\" + "log.txt"; 

                FileInfo fi = new(LogName);
                using FileStream fs = new(LogName, fi.Exists && fi.Length > Configuration.GetSection("Options").GetValue<long>("MaxLogSize") ? FileMode.Create : FileMode.OpenOrCreate);
                fs.Seek(0, SeekOrigin.End);

                using StreamWriter bw = new(fs);
                bw.Write(DateTime.Now + " ");
                
                string HostName = "";
                IPAddress? ip = context.HttpContext.Connection.RemoteIpAddress;
                try
                {
                    HostName = ip == null ? "" : " (" + Dns.GetHostEntry(ip).HostName + ")";
                }
                catch (Exception ex)
                {
                    HostName = " (" + ex.Message + ")";
                }

                bw.Write(ip + HostName + " ");
                bw.Write(context.HttpContext.Request.Path + "\r\n");

                bw.Close();
                fs.Close();
            }
            catch
            {
            }

            await next();
        }
    }
}
