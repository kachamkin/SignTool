using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class ConvertController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public ConvertController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Convert()
        {
            return View(new ConvertModel());
        }

        [HttpPost]
        public async Task<IActionResult> ConvertAsync(string? Password, string Method)
        {
            if (!ModelState.IsValid)
                return Global.GetErrors(this);
            else
            {
                if (Request.Form.Files.Count != 1)
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

                    await Task.Run(() => new CryptLib.CryptLib().ConvertCertFormat(certDir + "\\" + Request.Form.Files[0].FileName, Method, out _, Request.Form.Files.Count > 1 ? certDir + "\\" + Request.Form.Files[1].FileName : "", Password ?? ""));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        if (Method == "To CRT")
                            Global.PackFile(zip, certDir, "Certificate.crt");
                        else if (Method == "To CER")
                            Global.PackFile(zip, certDir, "Certificate.cer");
                        else if (Method.EndsWith("PFX"))
                            Global.PackFile(zip, certDir, "Certificate.pfx");
                        else if (Method == "PFX to PEM")
                        {
                            Global.PackFile(zip, certDir, "Certificate.pem");
                            Global.PackFile(zip, certDir, "Public.pem");
                            Global.PackFile(zip, certDir, "Private.pem");
                        }
                    }

                    return File(ms.ToArray(), "application/zip", "Certificates.zip");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message + (env.IsDevelopment() ? "\n" + ex.Source + "\n" + ex.StackTrace : ""));
                }
            }
        }

    }
}
