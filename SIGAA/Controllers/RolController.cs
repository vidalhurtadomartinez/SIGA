using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Models;
using MvcFlash.Core.Extensions;
using MvcFlash.Core;
using SIGAA.Commons;
using SIGAA.Etiquetas;

namespace SIGAA.Controllers
{
    [Autenticado]
    public class RolController : Controller
    {
        private SeguridadContext db = new SeguridadContext();

        [Permiso(Permiso = RolesPermisos.SEGU_rol_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var rol = db.Rol.Where(p => p.iEliminado_fl == 1).ToList();
            var rolFil = rol.Where(t => criterio == null ||
                                   t.Nombre.ToLower().Contains(criterio.ToLower()) ||
                                   t.Observacion.ToLower().Contains(criterio.ToLower()) 
                                   ).ToList();
            var rolFilOr = rolFil.OrderBy(ef => ef.Nombre);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + rolFilOr.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", rolFilOr);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + rolFilOr.Count() + " registros con los criterios especificados.");
            }
            return View(rolFilOr);
        }

        [Permiso(Permiso = RolesPermisos.SEGU_rol_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                Flash.Instance.Error("ERROR", "El paramettro Id no puede ser nulo");
                return RedirectToAction("Index");
            }
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                Flash.Instance.Warning("ERROR", "No Existe Rol con ID " + id);
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        [Permiso(Permiso = RolesPermisos.SEGU_rol_puedeCrearNuevo)]
        public ActionResult Create()
        {
            Rol rol = new Rol();
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iRol_id,Nombre,Observacion,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Rol rol)
        {
            try
            {
                rol.iEstado_fl = true;
                rol.iEliminado_fl = 1;
                rol.sCreado_by = FrontUser.Get().EmailUtepsa;
                rol.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Rol.Add(rol);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato se ha guradado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(rol);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido el siguiente error " + ex.Message);
                return RedirectToAction("Index");
            }          
        }

        [Permiso(Permiso = RolesPermisos.SEGU_rol_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iRol_id,Nombre,Observacion,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Rol rol)
        {
            try
            {
                rol.iEliminado_fl = 1;
                rol.sCreado_by = FrontUser.Get().EmailUtepsa;
                rol.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(rol).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido modificado correctamente");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(rol);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato porque ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
           
        }

        [Permiso(Permiso = RolesPermisos.SEGU_rol_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaccion = db.Database.BeginTransaction()) {
                try
                {
                    Rol rol = db.Rol.Find(id);
                    rol.iEstado_fl = false;
                    rol.iEliminado_fl = 2;
                    rol.sCreado_by = FrontUser.Get().EmailUtepsa;
                    rol.iConcurrencia_id += 1;

                    db.Entry(rol).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Rol.Remove(rol);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO","El dato ha sido eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR","No se pudo eliminar el dato porque ha ocurrido el siguiente error : "+ex.Message);
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
