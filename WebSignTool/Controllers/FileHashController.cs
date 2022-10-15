using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class FileHashController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public FileHashController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult FileHash()
        {
            return View(new FileHashModel());
        }

        [HttpPost]
        public async Task<IActionResult> FileHashAsync(string Method, string Output)
        {
            if (!ModelState.IsValid)
                return Global.GetErrors(this);
            else
            {
                if (Request.Form.Files.Count < 2)
                    return Content("Invalid count of files received!");

                try
                {
                    string certDir = Global.GetCertDir();
                    await Global.WriteFiles(Request.Form.Files, certDir);

                    if (Output == "Hex")
                        return Content(BitConverter.ToString(Convert.FromBase64String(await Task<string>.Run(() => new CryptLib.CryptLib().ComputeFileHash(certDir + "\\" + Request.Form.Files[0].FileName, Method, out _)))));
                    else
                        return Content(await Task<string>.Run(() => new CryptLib.CryptLib().ComputeFileHash(certDir + "\\" + Request.Form.Files[0].FileName, Method, out _)));
                }
                catch (Exception ex)
                {
                    return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
                }
            }
        }

    }
}
