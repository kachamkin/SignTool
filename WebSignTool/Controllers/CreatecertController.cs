using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.FileProviders;
using System.IO.Compression;
using System.Net.Http.Headers;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class CreatecertController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public CreatecertController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Createcert()
        {
            return View(new CreatecertModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreatecertAsync(string Issuer, string? Password, string AsymmetricAlgorithm, string HashAlgorithm, uint Valid)
        {
            if (!ModelState.IsValid)
                return Global.GetErrors(this);
            else
            {
                try
                {
                    string certDir = Global.GetCertDir();
                    await Task.Run(() => new CryptLib.CryptLib().CreateCertificate(certDir, Issuer, AsymmetricAlgorithm, HashAlgorithm, Password ?? "", (int)Valid));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        Stream zs = zip.CreateEntry(Issuer + ".pfx").Open();
                        byte[] buffer = System.IO.File.ReadAllBytes(certDir + "\\" + Issuer + ".pfx");
                        zs.Write(buffer, 0, buffer.Length);
                        zs.Close();

                        zs = zip.CreateEntry(Issuer + ".cer").Open();
                        buffer = System.IO.File.ReadAllBytes(certDir + "\\" + Issuer + ".cer");
                        zs.Write(buffer, 0, buffer.Length);
                        zs.Close();

                        zs.Dispose();
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
