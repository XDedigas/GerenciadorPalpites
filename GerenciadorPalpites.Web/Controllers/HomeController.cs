using System.Web.Mvc;

namespace GerenciadorPalpites.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Sobre()
        {
            return View();
        }

        [Authorize]
        public ActionResult Regulamento()
        {
            return View();
        }

        [Authorize]
        public ActionResult PerguntasFrequentes()
        {
            return View();
        }
    }
}