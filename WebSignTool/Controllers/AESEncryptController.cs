using Microsoft.AspNetCore.Mvc;
using System;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class AESEncryptController : Controller
    {
        private Microsoft.Extensions.Hosting.IHostEnvironment env;
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

                    foreach (IFormFile file in Request.Form.Files)
                    {
                        using FileStream stream = new(certDir + "\\" + file.FileName, FileMode.Create);
                        await file.CopyToAsync(stream);
                        stream.Close();
                    }

                    string IV = "";
                    await Task<string>.Run(() => new CryptLib.CryptLib().AESEncrypt(Output == "Hex" ? Convert.ToBase64String(Convert.FromHexString(Key.Replace("-", ""))) : Key, certDir + "\\" + Request.Form.Files[0].FileName, out IV));
                    if (Output == "Hex")
                        IV = BitConverter.ToString(Convert.FromBase64String(IV));

                    System.IO.File.Create(certDir + "\\IV.Txt").Close();
                    System.IO.File.WriteAllText(certDir + "\\IV.txt", IV);

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        Stream zs;

                        zs = zip.CreateEntry(Request.Form.Files[0].FileName).Open();
                        byte[] buffer = System.IO.File.ReadAllBytes(certDir + "\\" + Request.Form.Files[0].FileName);
                        zs.Write(buffer, 0, buffer.Length);
                        zs?.Close();

                        zs = zip.CreateEntry("IV.txt").Open();
                        buffer = System.IO.File.ReadAllBytes(certDir + "\\IV.txt" );
                        zs.Write(buffer, 0, buffer.Length);
                        zs?.Close();

                        zs?.Dispose();
                    }

                    return File(ms.ToArray(), "application/zip", "Encrypted.zip");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message + (env.IsDevelopment() ? "\n" + ex.Source + "\n" + ex.StackTrace : ""));
                }
            }
        }

    }
}
