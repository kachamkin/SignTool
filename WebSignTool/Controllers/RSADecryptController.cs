using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class RSADecryptController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
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

                    foreach (IFormFile file in Request.Form.Files)
                    {
                        using FileStream stream = new(certDir + "\\" + file.FileName, FileMode.Create);
                        await file.CopyToAsync(stream);
                        stream.Close();
                    }

                    await Task<string>.Run(() => new CryptLib.CryptLib().RSADecrypt(certDir + "\\" + Request.Form.Files[0].FileName, certDir + "\\" + Request.Form.Files[1].FileName, Method, Password ?? ""));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        using Stream zs = zip.CreateEntry(Request.Form.Files[0].FileName).Open();
                        byte[] buffer = System.IO.File.ReadAllBytes(certDir + "\\" + Request.Form.Files[0].FileName);
                        zs.Write(buffer, 0, buffer.Length);
                        zs.Close();
                    }

                    return File(ms.ToArray(), "application/zip", "Decrypted.zip");

                }
                catch (Exception ex)
                {
                    return Content(ex.Message + (env.IsDevelopment() ? "\n" + ex.Source + "\n" + ex.StackTrace : ""));
                }
            }
        }

    }
}
