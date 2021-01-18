using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CurriculumWeb.Models;
using System.Configuration;
using Microsoft.Extensions.Options;

namespace CurriculumWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMailer _mailer;

        public HomeController(ILogger<HomeController> logger, IMailer mailer )
        {
            _logger = logger;
            _mailer = mailer;

        }

        public IActionResult Index()
        {
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

        public IActionResult Contacto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contacto(Contacto contacto)
        {
            try
            {
                _mailer.SendEmailAsync("federico.celico87@gmail.com", "Curriculum Web", _mailer.GenerateBodyMessage(contacto));

                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
          

        }
    }
}
