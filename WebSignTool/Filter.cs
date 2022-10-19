using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        private IConfiguration Configuration;
        public LogFilter(IConfiguration _config)
        {
            Configuration = _config;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            try
            {
                string certDir = Global.GetCertDir(); 

                FileInfo fi = new(certDir + "\\" + "log.txt");
                using FileStream fs = new(certDir + "\\" + "log.txt", fi.Exists && fi.Length > Configuration.GetSection("Options").GetValue<long>("MaxLogSize") ? FileMode.Create : FileMode.OpenOrCreate);
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
