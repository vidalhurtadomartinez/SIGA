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
using MvcFlash.Core.Extensions;
using MvcFlash.Core;
using SIGAA.Etiquetas;

namespace SIGAA.Controllers
{
    [Autenticado]
    public class PermisoController : Controller
    {
        private SeguridadContext db = new SeguridadContext();

        [Permiso(Permiso = RolesPermisos.SEGU_permiso_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var perm = db.Permiso.Where(p => p.iEliminado_fl == 1).ToList();
            var permFil = perm.Where(t => criterio == null ||
                                   t.Modulo.ToLower().Contains(criterio.ToLower()) ||
                                   t.Nemonico.ToLower().Contains(criterio.ToLower()) ||
                                   t.Descripcion.ToLower().Contains(criterio.ToLower())
                                   ).ToList();
            var permFilOr = permFil.OrderBy(ef => ef.Modulo);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + permFilOr.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", permFilOr);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + permFilOr.Count() + " registros con los criterios especificados.");
            }
            return View(permFilOr);
        }

        [Permiso(Permiso =RolesPermisos.SEGU_permiso_puedeVerDetalle)]
        public ActionResult Details(RolesPermisos id)
        {
            if (id.Equals(null))
            {

                Flash.Instance.Error("ERROR", "El paramettro Id no puede ser nulo");
                return RedirectToAction("Index");
            }
            Permiso permiso = db.Permiso.Find(id);
            if (permiso == null)
            {
                Flash.Instance.Warning("ERROR", "No Existe Usuario con ID " + id);
                return RedirectToAction("Index");
            }
            return View(permiso);
        }

        [Permiso(Permiso =RolesPermisos.SEGU_permiso_puedeCrearNuevo)]
        public ActionResult Create()
        {
           // var prueba = ObtenerObjetoPermisoApartirDeUnaConstanteEnumRolesPermisos(RolesPermisos.EGRE_actaDeDefensaFinal_puedeCrearNuevo);
            CargarDDlistPermisos(0);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iPermiso_id,Modulo,Nemonico,Proceso,Descripcion,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Permiso permiso)
        {
            try
            {
                permiso.iEstado_fl = true;
                permiso.iEliminado_fl = 1;
                permiso.sCreado_by = FrontUser.Get().usr_login;
                permiso.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Permiso.Add(permiso);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO","El dato se ha guradado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo guardar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarDDlistPermisos(permiso.iPermiso_id);
                return View(permiso);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido el siguiente error :"+ex.Message);
                return RedirectToAction("Index");
            }
           
        }

        [Permiso(Permiso = RolesPermisos.SEGU_permiso_puedeEditar)]
        public ActionResult Edit(RolesPermisos id)
        {
            if (id.Equals(null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permiso permiso = db.Permiso.Find(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            CargarDDlistPermisos(id);
            return View(permiso);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iPermiso_id,Modulo,Nemonico,Proceso,Descripcion,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Permiso permiso)
        {
            try
            {
                permiso.iEliminado_fl = 1;
                permiso.sCreado_by = FrontUser.Get().usr_login;
                permiso.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(permiso).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido Modificado correctamente");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarDDlistPermisos(permiso.iPermiso_id);
                return View(permiso);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato proque ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }          
        }

        [Permiso(Permiso = RolesPermisos.SEGU_permiso_puedeEliminar)]
        public ActionResult Delete(RolesPermisos id)
        {
            if (id.Equals(null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Permiso permiso = db.Permiso.Find(id);
            if (permiso == null)
            {
                return HttpNotFound();
            }
            return View(permiso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RolesPermisos id)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    Permiso permiso = db.Permiso.Find(id);
                    permiso.iEstado_fl = false;
                    permiso.iEliminado_fl = 2;
                    permiso.sCreado_by = FrontUser.Get().usr_login;
                    permiso.iConcurrencia_id += 1;

                    db.Entry(permiso).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Permiso.Remove(permiso);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR", "No se pudo eliminar el dato porque ha ocurrido el siguiente error: " + ex.Message);
                    transaccion.Rollback();
                    return RedirectToAction("Index");
                }
            }
        }

        private void CargarDDlistPermisos(RolesPermisos selecionado = 0) {
            var permisoBD = db.Permiso.Select(p => p.iPermiso_id).Cast<RolesPermisos>().ToList();
            var permisoEnum = Enum.GetValues(typeof(RolesPermisos)).Cast<RolesPermisos>().ToList();

            var permisosNoEnBD = (from perE in permisoEnum
                                  where !permisoBD.Any(m => m.Equals(perE))
                                  select new { id= (int)perE,nombre= perE.ToString()}).ToList();
            if (selecionado > 0) {
                permisosNoEnBD.Add( new {
                                         id = (int)(RolesPermisos)Enum.ToObject(typeof(RolesPermisos), selecionado),
                                         nombre = ((RolesPermisos)Enum.ToObject(typeof(RolesPermisos), selecionado)).ToString()
                                        });
            }
            ViewBag.iPermiso_id = selecionado >0? new SelectList(permisosNoEnBD,"id","nombre",selecionado): new SelectList(permisosNoEnBD,"id","nombre");
        }

        public ActionResult TraerInformacionDePermiso_json(int rolPermiso_id)
        {
            var rolPermisoEnum = (RolesPermisos)Enum.ToObject(typeof(RolesPermisos), rolPermiso_id);
            var result = ObtenerObjetoPermisoApartirDeUnaConstanteEnumRolesPermisos(rolPermisoEnum);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Permiso ObtenerObjetoPermisoApartirDeUnaConstanteEnumRolesPermisos(RolesPermisos rolpermiso) {
            Permiso permiso = new Permiso();
            String nombreCompleto = Enum.GetName(typeof(RolesPermisos), rolpermiso);
            String[] palabras = nombreCompleto.Split('_');
            String modulo="";
            String nemonico="";
            String proceso="";
            String metodoDeAccion="";

            for (int i = 0; i < palabras.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        nemonico = palabras[i].Trim();
                        break;
                    case 1:
                        proceso = palabras[i].Trim();
                        break;
                    case 2:
                        metodoDeAccion = palabras[i].Trim();
                        break;
                }
            }

            modulo = ObtenerNombreModuloConNemonico(nemonico);

            permiso.Descripcion = nombreCompleto;
            permiso.iConcurrencia_id = 1;
            permiso.iEliminado_fl = 1;
            permiso.iEstado_fl = true;
            permiso.iPermiso_id = 0;
            permiso.Modulo = modulo;
            permiso.Nemonico = nemonico;
            permiso.Proceso = proceso;
            permiso.sCreado_by = FrontUser.Get().usr_login;

            return permiso;
        }

        private String ObtenerNombreModuloConNemonico(String nemonico) {
            Permiso permiso = db.Permiso.Where(p => p.Nemonico.Equals(nemonico)).FirstOrDefault();
            String nombreModulo = permiso != null ? permiso.Modulo : "";
            return nombreModulo;
        }

        //private String SepararPalablasPorLetraMayuscula(String palablasJuntas)
        //{
        //    //var string cadenaResultado;
        //    //var letras_mayusculas = "ABCDEFGHYJKLMNÑOPQRSTUVWXYZ";
        //    //for (int i = 0; i < palablasJuntas.Length; i++)
        //    //    {
        //    //        if (letras_mayusculas.IndexOf(palablasJuntas.ElementAt(i), 0) != -1)
        //    //        {
        //    //            return 1;
        //    //        }
        //    //    }


        //    //var res = palablasJuntas.CompareTo()
        //    //String nombreModulo = permiso != null ? permiso.Modulo : "";
        //    //return nombreModulo;
        //}

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
