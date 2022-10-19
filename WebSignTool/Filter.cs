using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            try
            {
                string certDir = Global.GetCertDir();

                FileInfo fi = new(certDir + "\\" + "log.txt");
                using FileStream fs = new(certDir + "\\" + "log.txt", fi.Exists && fi.Length > 1048576 ? FileMode.Create : FileMode.OpenOrCreate);
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

                bw.Write(context.HttpContext.Connection.RemoteIpAddress + HostName + " ");
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
