using AquaSense.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AquaSense.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            UsuarioViewModel usuario = HttpContext.Session.GetObject<UsuarioViewModel>("Usuario");
            if (usuario != null)
            {
                ViewBag.LoginUsuario = usuario.LoginUsuario.ToUpper();
                ViewBag.NomePessoa = usuario.NomePessoa.ToUpper();
                ViewBag.ImagemBase64 = usuario.ImagemEmBase64;
                ViewBag.Adm = usuario.Adm;
            }
            else
                return RedirectToAction("Index", "Login");

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
