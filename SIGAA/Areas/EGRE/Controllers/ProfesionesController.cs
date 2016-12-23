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
    public class ProfesionesController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_profesionTutor_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var profesiones = db.Profesiones.Where(p => p.iEliminado_fl == 1).ToList();
            var profesionesFiltrados = profesiones.Where(p => criterio == null || p.sNombre_nm.ToLower().Contains(criterio.ToLower())).ToList();
            var profesionesFiltradosOrdenadas = profesionesFiltrados.OrderBy(ef => ef.sNombre_nm);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + profesionesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", profesionesFiltradosOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + profesionesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(profesionesFiltradosOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_profesionTutor_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Profesiones gatbl_Profesiones = db.Profesiones.Find(id);
            if (gatbl_Profesiones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Profesiones);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_profesionTutor_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_Profesiones Profesion = new gatbl_Profesiones();
            return View(Profesion);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iProfesion_id,sNombre_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Profesiones Profesion)
        {
            try
            {
                Profesion.iEstado_fl = true;
                Profesion.iEliminado_fl = 1;
                Profesion.sCreado_by = FrontUser.Get().EmailUtepsa;
                Profesion.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Profesiones.Add(Profesion);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(Profesion);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }

        }

        [Permiso(Permiso = RolesPermisos.EGRE_profesionTutor_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Profesiones gatbl_Profesiones = db.Profesiones.Find(id);
            if (gatbl_Profesiones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Profesiones);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iProfesion_id,sNombre_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Profesiones Profesion)
        {
            Profesion.iEliminado_fl = 1;
            Profesion.sCreado_by = FrontUser.Get().EmailUtepsa;
            Profesion.iConcurrencia_id += 1;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(Profesion).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(Profesion);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_profesionTutor_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Profesiones Profesion = db.Profesiones.Find(id);
            if (Profesion == null)
            {
                return HttpNotFound();
            }
            return View(Profesion);
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
                    gatbl_Profesiones Profesion = db.Profesiones.Find(id);
                    Profesion.iEstado_fl = false;
                    Profesion.iEliminado_fl = 2;
                    Profesion.sCreado_by = FrontUser.Get().EmailUtepsa;
                    Profesion.iConcurrencia_id += 1;

                    db.Entry(Profesion).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Profesiones.Remove(Profesion);
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
