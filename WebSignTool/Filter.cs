﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace WebSignTool
{
    public class LogFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string certDir = Global.GetCertDir();

            using (FileStream fs = new(certDir + "\\" + "log.txt", FileMode.OpenOrCreate))
            {
                fs.Seek(0, SeekOrigin.End);
                using (StreamWriter bw = new(fs))
                {
                    await bw.WriteAsync(DateTime.Now + " ");
                    await bw.WriteAsync(context.HttpContext.Request.Host + " ");
                    await bw.WriteAsync(context.HttpContext.Request.Path + "\n");
                    bw.Close();
                }
                fs.Close();
            }

            await next();
        }
    }
}
