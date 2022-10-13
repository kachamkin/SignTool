using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class SignappController : Controller
    {
        public IActionResult Signapp()
        {
            return View(new SignappModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignappAsync(string? Password, bool IsAppX)
        {
            if (!ModelState.IsValid)
                return Global.GetErrors(this);
            else
            {
                try
                {
                    string certDir = Global.GetCertDir();

                    foreach (IFormFile file in Request.Form.Files)
                    {
                        using FileStream stream = new(certDir + "\\" + file.FileName, FileMode.Create);
                        await file.CopyToAsync(stream);
                        stream.Close();
                    }

                    await Task.Run(() => new CryptLib.CryptLib().SignApplication(certDir + "\\" + Request.Form.Files[0].FileName, certDir + "\\" + Request.Form.Files[1].FileName, Password ?? "", IsAppX));

                    using MemoryStream ms = new();
                    using (ZipArchive zip = new(ms, ZipArchiveMode.Create))
                    {
                        using Stream zs = zip.CreateEntry(Request.Form.Files[0].FileName).Open();
                        byte[] buffer = System.IO.File.ReadAllBytes(certDir + "\\" + Request.Form.Files[0].FileName);
                        zs.Write(buffer, 0, buffer.Length);
                        zs.Close();
                    }

                    return File(ms.ToArray(), "application/zip", "Signed.zip");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
                }
            }
        }

    }
}
