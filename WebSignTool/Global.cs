﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.IO.Compression;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public static void OnTelegramMessageReceived(string message, ITelegram telegram)
        {
            if (!string.IsNullOrEmpty(message))
            {
                telegram.SendMessage("Your message is: " + message);
                try
                {
                    string certDir = Global.GetCertDir();
                    System.IO.File.Open(certDir + "\\updateid.txt", FileMode.OpenOrCreate).Close();
                    System.IO.File.WriteAllTextAsync(certDir + "\\updateid.txt", telegram.UpdateId.ToString());
                }
                catch { }

            }
        }

        public static void AddTelegram(WebApplicationBuilder builder)
        {
            int updateId = 0;
            try
            {
                string certDir = Global.GetCertDir();
                System.IO.File.Open(certDir + "\\updateid.txt", FileMode.Open).Close();
                updateId = int.Parse(System.IO.File.ReadAllText(certDir + "\\updateid.txt"));
            }
            catch { }

            IConfigurationSection options = builder.Configuration.GetSection("Options");
            Telegram telegram = new(options.GetValue<string>("TelegramToken"), options.GetValue<int>("TelegramId"), updateId);
            telegram.OnMessageReceived += Global.OnTelegramMessageReceived;

            builder.Services.AddSingleton<ITelegram>(telegram);
        }
    }
}
