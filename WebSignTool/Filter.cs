using Microsoft.AspNetCore.Mvc.Filters;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            try
            {

                string certDir = Global.GetCertDir();

                using (FileStream fs = new(certDir + "\\" + "log.txt", FileMode.OpenOrCreate))
                {
                    fs.Seek(0, SeekOrigin.End);
                    using (StreamWriter bw = new(fs))
                    {
                        bw.Write(DateTime.Now + " ");
                        //IPHostEntry? host = context.HttpContext.Connection.RemoteIpAddress == null ? null : Dns.GetHostEntry(context.HttpContext.Connection.RemoteIpAddress);
                        //bw.Write(context.HttpContext.Connection.RemoteIpAddress + (host == null ? "" : " (" + host.HostName + ")") + " ");
                        bw.Write(context.HttpContext.Connection.RemoteIpAddress + " ");
                        bw.Write(context.HttpContext.Request.Path + "\r\n");
                        bw.Close();
                    }
                    fs.Close();
                }
            }
            catch
            {
            }

            await next();
        }
    }
}
