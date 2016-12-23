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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iPermiso_id,Modulo,Nemonico,Descripcion,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Permiso permiso)
        {
            try
            {
                permiso.iEstado_fl = true;
                permiso.iEliminado_fl = 1;
                permiso.sCreado_by = FrontUser.Get().EmailUtepsa;
                permiso.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Permiso.Add(permiso);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO","El dato se ha guradado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo guardar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
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
            return View(permiso);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iPermiso_id,Modulo,Nemonico,Descripcion,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Permiso permiso)
        {
            try
            {
                permiso.iEliminado_fl = 1;
                permiso.sCreado_by = FrontUser.Get().EmailUtepsa;
                permiso.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(permiso).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido Modificado correctamente");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
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
                    Permiso permiso = db.Permiso.Find((int)id);
                    permiso.iEstado_fl = false;
                    permiso.iEliminado_fl = 2;
                    permiso.sCreado_by = FrontUser.Get().EmailUtepsa;
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
