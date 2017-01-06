using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Models;
using SIGAA.Commons;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using SIGAA.Etiquetas;

namespace SIGAA.Controllers
{
    [Autenticado]
    public class HistorialCambioRolDeUsuarioController : Controller
    {
        private SeguridadContext db = new SeguridadContext();

        [Permiso(Permiso = RolesPermisos.SEGU_HistorialCambioRolDeUsuario_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var hist = db.HistorialCambioRolDeUsuarios.Where(p => p.iEliminado_fl == 1).Include(r => r.RolAnterior).Include(r => r.RolActual).Include(r => r.Usuario).ToList();
            var histFil = hist.Where(t => criterio == null ||
                                   t.Usuario.usr_login.ToLower().Contains(criterio.ToLower()) ||
                                   t.RolAnterior.Nombre.ToLower().Contains(criterio.ToLower()) ||
                                   t.RolActual.Nombre.ToLower().Contains(criterio.ToLower()) ||
                                   t.dtFechaCambio.ToString().Contains(criterio.ToLower()) ||
                                   t.sCreado_by.ToLower().Contains(criterio.ToLower())
                                   ).ToList();
            var histFilOr = histFil.OrderByDescending(ef => ef.dtFechaCambio);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + histFilOr.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", histFilOr);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + histFilOr.Count() + " registros con los criterios especificados.");
            }
            return View(histFilOr);
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
