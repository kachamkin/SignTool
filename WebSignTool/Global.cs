using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.IO.Compression;
using System.Net;

namespace WebSignTool
{
    public static class Global
    {
        public static IActionResult GetErrors(Controller c)
        {
            string errorMessages = "";
            foreach (var item in c.ModelState)
            {
                if (item.Value.ValidationState == ModelValidationState.Invalid)
                {
                    errorMessages = $"{errorMessages}\nErrors for {item.Key}:\n";
                    foreach (var error in item.Value.Errors)
                        errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
                }
            }
            return c.Content(errorMessages);
        }

        public static string GetCertDir()
        {
            string certDir = Path.GetTempPath() + "WebSigTool";
            if (!Directory.Exists(certDir))
                Directory.CreateDirectory(certDir);

            return certDir;
        }

        public static void PackFile(ZipArchive zip, string dir, string file)
        {
            using Stream zs = zip.CreateEntry(file).Open();
            byte[] buffer = System.IO.File.ReadAllBytes(dir + "\\" + file);
            zs.Write(buffer, 0, buffer.Length);
            zs.Close();
        }

        public static string ErrorMessage(Exception ex, bool IsDevelopment)
        {
            return ex.Message + (IsDevelopment ? "\n" + ex.Source + "\n" + ex.StackTrace : "");
        }

        public static async Task WriteFiles(IFormFileCollection Files, string dir)
        {
            foreach (IFormFile file in Files)
            {
                using FileStream stream = new(dir + "\\" + file.FileName, FileMode.Create);
                await file.CopyToAsync(stream);
                stream.Close();
            }
        }

        public static void SendLogMessageByTelegram(string message, IConfiguration config)
        {
            new HttpClient().GetStringAsync(
                "https://api.telegram.org/bot" + 
                config.GetSection("Options").GetValue<string>("TelegramToken") + 
                "/sendMessage?chat_id=" + 
                config.GetSection("Options").GetValue<string>("TelegramId") + 
                "&text=" + message
                );
        }
    }
}
