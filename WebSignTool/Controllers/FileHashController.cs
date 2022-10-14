using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class FileHashController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
        public FileHashController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult FileHash()
        {
            return View(new FileHashModel());
        }

        [HttpPost]
        public async Task<IActionResult> FileHashAsync(IFormFile upFile, string Method, string Output)
        {
            if (!ModelState.IsValid)
                return Global.GetErrors(this);
            else
            {
                try
                {
                    string certDir = Global.GetCertDir();
                    
                    using (FileStream stream = new(certDir + "\\" + upFile.FileName, FileMode.Create))
                    {
                        await upFile.CopyToAsync(stream);
                        stream.Close();
                    }

                    if (Output == "Hex")
                        return Content(BitConverter.ToString(Convert.FromBase64String(await Task<string>.Run(() => new CryptLib.CryptLib().ComputeFileHash(certDir + "\\" + upFile.FileName, Method, out _)))));
                    else
                        return Content(await Task<string>.Run(() => new CryptLib.CryptLib().ComputeFileHash(certDir + "\\" + upFile.FileName, Method, out _)));
                }
                catch (Exception ex)
                {
                    return Content(ex.Message + (env.IsDevelopment() ? "\n" + ex.Source + "\n" + ex.StackTrace : ""));
                }
            }
        }

    }
}
