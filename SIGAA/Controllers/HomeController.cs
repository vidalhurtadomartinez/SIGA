using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Controllers
{
    [Autenticado]
    public class HomeController : Controller
    {
        [Permiso(Permiso = RolesPermisos.SEGU_home_puedeVerIndex)]
        public ActionResult Index()
        {
            return View();
        }
    }
}