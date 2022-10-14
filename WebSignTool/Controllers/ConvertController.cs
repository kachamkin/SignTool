using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class ConvertController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
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
                        Stream zs;
                        byte[] buffer;
                        if (Method == "To CRT")
                        {
                            zs = zip.CreateEntry("Certificate.crt").Open();
                            buffer = System.IO.File.ReadAllBytes(certDir + "\\Certificate.crt");
                            zs.Write(buffer, 0, buffer.Length);
                            zs.Close();
                        }
                        else if (Method == "To CER")
                        {
                            zs = zip.CreateEntry("Certificate.cer").Open();
                            buffer = System.IO.File.ReadAllBytes(certDir + "\\Certificate.cer");
                            zs.Write(buffer, 0, buffer.Length);
                            zs.Close();
                        }
                        else if (Method.EndsWith("PFX"))
                        {
                            zs = zip.CreateEntry("Certificate.pfx").Open();
                            buffer = System.IO.File.ReadAllBytes(certDir + "\\Certificate.pfx");
                            zs.Write(buffer, 0, buffer.Length);
                            zs.Close();
                        }
                        else if (Method == "PFX to PEM")
                        {
                            zs = zip.CreateEntry("Certificate.pem").Open();
                            buffer = System.IO.File.ReadAllBytes(certDir + "\\Certificate.pem");
                            zs.Write(buffer, 0, buffer.Length);
                            zs.Close();

                            zs = zip.CreateEntry("Public.pem").Open();
                            buffer = System.IO.File.ReadAllBytes(certDir + "\\Public.pem");
                            zs.Write(buffer, 0, buffer.Length);
                            zs.Close();

                            zs = zip.CreateEntry("Private.pem").Open();
                            buffer = System.IO.File.ReadAllBytes(certDir + "\\Private.pem");
                            zs.Write(buffer, 0, buffer.Length);
                            zs.Close();
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
