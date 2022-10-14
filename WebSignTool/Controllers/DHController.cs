using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class DHController : Controller
    {
        private readonly Microsoft.Extensions.Hosting.IHostEnvironment env;
        public DHController(Microsoft.Extensions.Hosting.IHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Dh()
        {
            return View(new DHModel());
        }

        [HttpPost]
        public async Task<IActionResult> DhAsync(string? Password, string Output, string Method)
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

                    if (Output == "Hex")
                        return Content(BitConverter.ToString(Convert.FromBase64String(await Task<string>.Run(() => new CryptLib.CryptLib().GenerateECDiffieHellmanKey(certDir + "\\" + Request.Form.Files[1].FileName, certDir + "\\" + Request.Form.Files[0].FileName, Method, Password ?? "")))));
                    else
                        return Content(await Task<string>.Run(() => new CryptLib.CryptLib().GenerateECDiffieHellmanKey(certDir + "\\" + Request.Form.Files[1].FileName, certDir + "\\" + Request.Form.Files[0].FileName, Method, Password ?? "")));
                }
                catch (Exception ex)
                {
                    return Content(ex.Message + (env.IsDevelopment() ? "\n" + ex.Source + "\n" + ex.StackTrace : ""));
                }
            }
        }

    }
}
