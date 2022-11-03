using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;
using WebSignTool.Models;

namespace WebSignTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbit rabbit;
        private readonly IWebSocket chat; 

        public HomeController(ILogger<HomeController> logger, IRabbit _rabbit, IWebSocket _chat)
        {
            _logger = logger;
            rabbit = _rabbit;
            chat = _chat;
        }

        public IActionResult Index()
        {
            try
            {
                string ret = "";
                foreach (var h in HttpContext.Request.Headers)
                    ret += h.Key + ": " + h.Value + "\r\n\r\n";
                rabbit.SendMessage(ret);
                chat.SendMessage(ret);
            }
            catch { };

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}