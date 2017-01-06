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
    public class PresidenteColegioProfesionalController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_presidenteColProfesional_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var presidentesColegiosProfesionales = db.PresidentesColegiosProfesionales.Where(p => p.iEliminado_fl ==1).Include(g => g.ColegioProfesional);
            var PresidentesFiltrados = presidentesColegiosProfesionales.Where(t => criterio == null ||                                                                     
                                                                     t.sNombreCompleto_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                     t.sEstadoActivo_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                     t.sTelefono_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                     t.sEmail_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                     t.sEmail_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                     t.ColegioProfesional.sNombre_nm.ToLower().Contains(criterio.ToLower())
                                                                     ).ToList();
            var PresidentesFiltradosOrdenadas = PresidentesFiltrados.OrderBy(ef => ef.sNombreCompleto_desc).ThenBy(ef => ef.ColegioProfesional.sNombre_nm);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + PresidentesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", PresidentesFiltradosOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + PresidentesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(PresidentesFiltradosOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_presidenteColProfesional_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PresidenteColegioProfesional PresidenteColegioProfesional = db.PresidentesColegiosProfesionales.Find(id);
            if (PresidenteColegioProfesional == null)
            {
                return HttpNotFound();
            }
            return View(PresidenteColegioProfesional);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_presidenteColProfesional_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_PresidenteColegioProfesional presidenteColegioProfesional = new gatbl_PresidenteColegioProfesional();
            CargarViewBags(Tarea.NUEVO, presidenteColegioProfesional);
            return View(presidenteColegioProfesional);
        }

        //CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iPresidenteColProfesional_id,sNombreCompleto_desc,sNroDocumento_desc,dtFefhaInicioCargo_dt,dtFechaFinCargo_dt,sEstadoActivo_fl,sTelefono_desc,sEmail_desc,iCProfesional_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_PresidenteColegioProfesional PresidenteColegioProfesional)
        {            
            try
            {
                PresidenteColegioProfesional.iEstado_fl = true;
                PresidenteColegioProfesional.iEliminado_fl = 1;
                PresidenteColegioProfesional.sCreado_by = FrontUser.Get().usr_login;
                PresidenteColegioProfesional.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.PresidentesColegiosProfesionales.Add(PresidenteColegioProfesional);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, PresidenteColegioProfesional);
                return View(PresidenteColegioProfesional);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_presidenteColProfesional_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PresidenteColegioProfesional PresidenteColegioProfesional = db.PresidentesColegiosProfesionales.Find(id);
            if (PresidenteColegioProfesional == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, PresidenteColegioProfesional);
            return View(PresidenteColegioProfesional);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iPresidenteColProfesional_id,sNombreCompleto_desc,sNroDocumento_desc,dtFefhaInicioCargo_dt,dtFechaFinCargo_dt,sEstadoActivo_fl,sTelefono_desc,sEmail_desc,iCProfesional_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_PresidenteColegioProfesional PresidenteColegioProfesional)
        {           
            try
            {
                PresidenteColegioProfesional.iEliminado_fl = 1;
                PresidenteColegioProfesional.sCreado_by = FrontUser.Get().usr_login;
                PresidenteColegioProfesional.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(PresidenteColegioProfesional).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tarea.EDITAR, PresidenteColegioProfesional);               
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(PresidenteColegioProfesional);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_presidenteColProfesional_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PresidenteColegioProfesional PresidenteColegioProfesional = db.PresidentesColegiosProfesionales.Find(id);
            if (PresidenteColegioProfesional == null)
            {
                return HttpNotFound();
            }
            return View(PresidenteColegioProfesional);
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
                    gatbl_PresidenteColegioProfesional PresidenteColegioProfesional = db.PresidentesColegiosProfesionales.Find(id);
                    PresidenteColegioProfesional.iEstado_fl = false;
                    PresidenteColegioProfesional.iEliminado_fl = 2;
                    PresidenteColegioProfesional.sCreado_by = FrontUser.Get().usr_login;
                    PresidenteColegioProfesional.iConcurrencia_id += 1;

                    db.Entry(PresidenteColegioProfesional).State = EntityState.Modified;
                    db.SaveChanges();

                    db.PresidentesColegiosProfesionales.Remove(PresidenteColegioProfesional);
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

        private void CargarViewBags(Tarea tarea, gatbl_PresidenteColegioProfesional PresidentesColegiosProfesionales)
        {
            ViewBag.iCProfesional_id = new SelectList(db.Profesionales.Where(p => p.iEliminado_fl ==1), "iCProfesional_id", "sNombre_nm", PresidentesColegiosProfesionales.iCProfesional_id);
            if (tarea == Tarea.EDITAR)
            {
                //var receptor = TraerInformacionDePersonaPorCodigo(rectorUniversidadPublica.lRecepciona_id);
                //ViewBag.Receptor = receptor;
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
