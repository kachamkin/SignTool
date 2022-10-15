using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class VerifyController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public VerifyController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }
        public IActionResult Verify()
        {
            return View(new VerifyModel());
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAsync()
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
                    await Global.WriteFiles(Request.Form.Files, certDir);

                    string Signatures = "";
                    int count = 0;
                    await Task<string>.Run(() => new CryptLib.CryptLib().VerifySignature(certDir + "\\" + Request.Form.Files[0].FileName, out Signatures, out count));

                    System.IO.File.Create(certDir + "\\Signatures.txt").Close();
                    System.IO.File.WriteAllText(certDir + "\\Signatures.txt", Signatures);

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        Global.PackFile(zip, certDir, Request.Form.Files[0].FileName);
                        Global.PackFile(zip, certDir, "Signatures.txt");
                        for (int i = 0; i < count; i++)
                            Global.PackFile(zip, certDir, "Certificate" + (i + 1) + ".cer");
                    }

                    return File(ms.ToArray(), "application/zip", "Decoded.zip");
                }
                catch (Exception ex)
                {
                    return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
                }
            }
        }

    }
}
