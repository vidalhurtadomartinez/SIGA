using Helper;
using System.Web.Mvc;
using SIGAA.Models;
using SIGAA.ViewModels;
using SIGAA.Etiquetas;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;

namespace SIGAA.Controllers
{
    public class LoginController : Controller
    {
        private Usuario usu = new Usuario();

        [NoLogin]
        public ActionResult Autenticarse()
        {
            LoginViewModel loginVM = new LoginViewModel();
            return View(loginVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Autenticarse(LoginViewModel model)
        {        
            if (ModelState.IsValid)
            { 
                if (usu.Autenticarse(model.EmailUtepsa, model.Contrasena))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            Flash.Instance.Error("ERROR", "No se pudo Autenticar, puede que no tenga acceso al Sistema ó el email y contraseña no sean correctos.");
            return View(model);
        }

        [Autenticado]
        public ActionResult CerrarSession()
        {
            SessionHelper.CerrarSessionDeUsuario();
            return RedirectToAction("Autenticarse", "Login");
        }
    }
}