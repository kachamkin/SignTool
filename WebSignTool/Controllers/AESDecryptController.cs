using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class AESDecryptController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public AESDecryptController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }
        public IActionResult AESDecrypt()
        {
            return View(new AESDecryptModel());
        }

        [HttpPost]
        public async Task<IActionResult> AESDecryptAsync(string Output, string Key, string IV)
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

                    await Task<string>.Run(() => new CryptLib.CryptLib().AESDecrypt(Output == "Hex" ? Convert.ToBase64String(Convert.FromHexString(Key.Replace("-", ""))) : Key, Output == "Hex" ? Convert.ToBase64String(Convert.FromHexString(IV.Replace("-", ""))) : IV, certDir + "\\" + Request.Form.Files[0].FileName));

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
