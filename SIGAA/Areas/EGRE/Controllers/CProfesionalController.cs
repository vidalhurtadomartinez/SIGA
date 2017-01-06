using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using SIGAA.Areas.EGRE.Models;
using SIGAA.Commons;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.EGRE.Controllers
{
    [Autenticado]
    public class CProfesionalController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var CProfesionales = db.Profesionales.Where(p => p.iEliminado_fl == 1).ToList();
            var CProfesionalesFiltrados = CProfesionales.Where(cp => criterio == null ||
                                                               cp.sNombre_nm.ToLower().Contains(criterio.ToLower()) ||
                                                               cp.sDiereccion_desc.ToLower().Contains(criterio.ToLower()) ||
                                                               cp.sTelefono_desc.ToLower().Contains(criterio.ToLower()) ||
                                                               cp.sRepresentante_nm.ToLower().Contains(criterio.ToLower())
                                                               ).ToList();
            var CProfesionalesFiltradosOrdenadas = CProfesionalesFiltrados.OrderBy(ef => ef.sNombre_nm);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + CProfesionalesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", CProfesionalesFiltradosOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + CProfesionalesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(CProfesionalesFiltradosOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_CProfesionales CProfesionales = db.Profesionales.Find(id);
            if (CProfesionales == null)
            {
                return HttpNotFound();
            }
            return View(CProfesionales);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_CProfesionales CProfesional = new gatbl_CProfesionales();
            return View(CProfesional);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iCProfesional_id,sNombre_nm,sDiereccion_desc,sTelefono_desc,sRepresentante_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_CProfesionales CProfesional)
        {           
            try
            {
                CProfesional.iEstado_fl = true;
                CProfesional.iEliminado_fl = 1;
                CProfesional.sCreado_by = FrontUser.Get().usr_login;
                CProfesional.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Profesionales.Add(CProfesional);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(CProfesional);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " +ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_CProfesionales CProfesional = db.Profesionales.Find(id);
            if (CProfesional == null)
            {
                return HttpNotFound();
            }
            return View(CProfesional);
        }

        //EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iCProfesional_id,sNombre_nm,sDiereccion_desc,sTelefono_desc,sRepresentante_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_CProfesionales CProfesional)
        {            
            try
            {
                CProfesional.iEliminado_fl = 1;
                CProfesional.sCreado_by = FrontUser.Get().usr_login;
                CProfesional.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(CProfesional).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " +ex.Message);
                return View(CProfesional);
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_CProfesionales CProfesional = db.Profesionales.Find(id);
            if (CProfesional == null)
            {
                return HttpNotFound();
            }
            return View(CProfesional);
        }

        //ELIMINAR POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    gatbl_CProfesionales CProfesional = db.Profesionales.Find(id);
                    CProfesional.iEstado_fl = false;
                    CProfesional.iEliminado_fl = 2;
                    CProfesional.sCreado_by = FrontUser.Get().usr_login;
                    CProfesional.iConcurrencia_id += 1;

                    db.Entry(CProfesional).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Profesionales.Remove(CProfesional);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR:    ", "No se pudo Eliminar el dato, ha ocurrido el siguiente error: " + ex.Message);
                    transaccion.Rollback();
                    return RedirectToAction("Index");
                }
            }
        }

        //TEREAS
        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
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
