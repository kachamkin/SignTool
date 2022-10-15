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
                        Global.PackFile(zip, certDir, Issuer + ".pfx");
                        Global.PackFile(zip, certDir, Issuer + ".cer");
                    }

                    return File(ms.ToArray(), "application/zip", "Certificates.zip");
                }
                catch (Exception ex)
                {
                    return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
                }
            }
        }
    }
}
