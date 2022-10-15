using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class SignfileController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
        public SignfileController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Signfile()
        {
            return View(new SignfileModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignfileAsync(string? Password, bool Detached)
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
                    await Global.WriteFiles(Request.Form.Files, certDir);

                    await Task.Run(() => new CryptLib.CryptLib().SignFile(certDir + "\\" + Request.Form.Files[0].FileName, certDir + "\\" + Request.Form.Files[1].FileName, Detached, certDir + "\\" + Request.Form.Files[0].FileName + ".sig", Password ?? ""));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        Global.PackFile(zip, certDir, Request.Form.Files[0].FileName);
                        if (Detached)
                            Global.PackFile(zip, certDir, Request.Form.Files[0].FileName + ".sig");
                    }

                    return File(ms.ToArray(), "application/zip", "Signed.zip");
                }
                catch (Exception ex)
                {
                    return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
                }
            }
        }

    }
}
