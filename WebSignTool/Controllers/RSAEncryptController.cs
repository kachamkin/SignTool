﻿using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class RSAEncryptController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public RSAEncryptController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult RSAEncrypt()
        {
            return View(new RSAEncryptModel());
        }

        [HttpPost]
        public async Task<IActionResult> RSAEncryptAsync(string Method)
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

                    await Task<string>.Run(() => new CryptLib.CryptLib().RSAEncrypt(certDir + "\\" + Request.Form.Files[0].FileName, certDir + "\\" + Request.Form.Files[1].FileName, Method));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                        Global.PackFile(zip, certDir, Request.Form.Files[0].FileName);

                    return File(ms.ToArray(), "application/zip", "Encrypted.zip");

                }
                catch (Exception ex)
                {
                    return Content(Global.ErrorMessage(ex, env.IsDevelopment()));
                }
            }
        }

    }
}
