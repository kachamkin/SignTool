using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class SignappController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
        public SignappController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Signapp()
        {
            return View(new SignappModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignappAsync(string? Password, bool IsAppX)
        {
            if (!ModelState.IsValid)
                return Global.GetErrors(this);
            else
            {
                if (Request.Form.Files.Count != 2)
                    return Content("Invalid count of files received!");

                try
                {
                    string certDir = Global.GetCertDir();

                    foreach (IFormFile file in Request.Form.Files)
                    {
                        using FileStream stream = new(certDir + "\\" + file.FileName, FileMode.Create);
                        await file.CopyToAsync(stream);
                        stream.Close();
                    }

                    await Task.Run(() => new CryptLib.CryptLib().SignApplication(certDir + "\\" + Request.Form.Files[0].FileName, certDir + "\\" + Request.Form.Files[1].FileName, Password ?? "", IsAppX));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                        Global.PackFile(zip, certDir, Request.Form.Files[0].FileName);

                    return File(ms.ToArray(), "application/zip", "Signed.zip");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message + (env.IsDevelopment() ? "\n" + ex.Source + "\n" + ex.StackTrace : ""));
                }
            }
        }

    }
}
