using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class RSADecryptController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public RSADecryptController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult RSADecrypt()
        {
            return View(new RSADecryptModel());
        }

        [HttpPost]
        public async Task<IActionResult> RSADecryptAsync(string Method, string? Password)
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

                    await Task<string>.Run(() => new CryptLib.CryptLib().RSADecrypt(certDir + "\\" + Request.Form.Files[0].FileName, certDir + "\\" + Request.Form.Files[1].FileName, Method, Password ?? ""));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                        Global.PackFile(zip, certDir, Request.Form.Files[0].FileName);

                    return File(ms.ToArray(), "application/zip", "Decrypted.zip");

                }
                catch (Exception ex)
                {
                    return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
                }
            }
        }

    }
}
