﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class AESEncryptController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public AESEncryptController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }
        public IActionResult AESEncrypt()
        {
            return View(new AESEncryptModel());
        }

        [HttpPost]
        public async Task<IActionResult> AESEncryptAsync(string Output, string Key)
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

                    string IV = "";
                    await Task<string>.Run(() => new CryptLib.CryptLib().AESEncrypt(Output == "Hex" ? Convert.ToBase64String(Convert.FromHexString(Key.Replace("-", ""))) : Key, certDir + "\\" + Request.Form.Files[0].FileName, out IV));
                    if (Output == "Hex")
                        IV = BitConverter.ToString(Convert.FromBase64String(IV));

                    System.IO.File.Create(certDir + "\\IV.Txt").Close();
                    System.IO.File.WriteAllText(certDir + "\\IV.txt", IV);

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        Global.PackFile(zip, certDir, Request.Form.Files[0].FileName);
                        Global.PackFile(zip, certDir, "IV.txt");
                    }

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
