using SIGAA.Etiquetas;
using System.Web.Mvc;

namespace SIGAA.Areas.EGRE.Controllers
{
    [Autenticado]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Aplicacion de gestión y control de Egresados.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Página de contacto.";

            return View();
        }
    }
}