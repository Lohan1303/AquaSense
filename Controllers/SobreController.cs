using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Controllers
{
    public class SobreController : Controller
    {
        // Ação para a página inicial
        public ActionResult Index()
        {
            return View();
        }

        // Ação para a página de contato
        public ActionResult Contact()
        {
            return View();
        }
    }
}

