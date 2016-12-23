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
    public class RectorUniversidadPublicaController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_rectorUniversidaPublica_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var rectoresUniversidadesPublicas = db.RectoresUniversidadesPublicas.Where(p => p.iEliminado_fl == 1).Include(g => g.Universidad).ToList();
            var rectoresFiltrados = rectoresUniversidadesPublicas.Where(t => criterio == null ||
                                                                        t.Universidad.unv_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                        t.sNombreCompleto_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                        t.sTelefono_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                        t.sEmail_desc.ToLower().Contains(criterio.ToLower())
                                                                        ).ToList();
            var rectoresFiltradosOrdenadas = rectoresFiltrados.OrderBy(ef => ef.sNombreCompleto_desc).ThenBy(ef => ef.Universidad.unv_descripcion);

            if (Request.IsAjaxRequest()) {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + rectoresFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", rectoresFiltradosOrdenadas);
            }
            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + rectoresFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(rectoresFiltradosOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_rectorUniversidaPublica_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_RectorUniversidadPublica RectorUniversidadPublica = db.RectoresUniversidadesPublicas.Find(id);
            if (RectorUniversidadPublica == null)
            {
                return HttpNotFound();
            }
            return View(RectorUniversidadPublica);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_rectorUniversidaPublica_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_RectorUniversidadPublica RectorUniversidadPublica = new gatbl_RectorUniversidadPublica();           
            CargarViewBags(Tarea.NUEVO, RectorUniversidadPublica);
            return View(RectorUniversidadPublica);
        }

        //CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iRectorUnivPublica_id,sNombreCompleto_desc,sNroDocumento_desc,dtFefhaInicioCargo_dt,dtFechaFinCargo_dt,sEstadoActivo_fl,sTelefono_desc,sEmail_desc,unv_codigo,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_RectorUniversidadPublica RectorUniversidadPublica)
        {            
            try
            {
                RectorUniversidadPublica.iEstado_fl = true;
                RectorUniversidadPublica.iEliminado_fl = 1;
                RectorUniversidadPublica.sCreado_by = FrontUser.Get().EmailUtepsa;
                RectorUniversidadPublica.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.RectoresUniversidadesPublicas.Add(RectorUniversidadPublica);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, RectorUniversidadPublica);
                return View(RectorUniversidadPublica);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " +ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_rectorUniversidaPublica_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_RectorUniversidadPublica RectorUniversidadPublica = db.RectoresUniversidadesPublicas.Find(id);
            if (RectorUniversidadPublica == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, RectorUniversidadPublica);
            return View(RectorUniversidadPublica);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iRectorUnivPublica_id,sNombreCompleto_desc,sNroDocumento_desc,dtFefhaInicioCargo_dt,dtFechaFinCargo_dt,sEstadoActivo_fl,sTelefono_desc,sEmail_desc,unv_codigo,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_RectorUniversidadPublica RectorUniversidadPublica)
        {            
            try
            {
                RectorUniversidadPublica.iEliminado_fl = 1;
                RectorUniversidadPublica.sCreado_by = FrontUser.Get().EmailUtepsa;
                RectorUniversidadPublica.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(RectorUniversidadPublica).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tarea.EDITAR, RectorUniversidadPublica);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(RectorUniversidadPublica);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " +ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_rectorUniversidaPublica_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_RectorUniversidadPublica RectorUniversidadPublica = db.RectoresUniversidadesPublicas.Find(id);
            if (RectorUniversidadPublica == null)
            {
                return HttpNotFound();
            }
            return View(RectorUniversidadPublica);
        }

        //DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    gatbl_RectorUniversidadPublica RectorUniversidadPublica = db.RectoresUniversidadesPublicas.Find(id);
                    RectorUniversidadPublica.iEstado_fl = false;
                    RectorUniversidadPublica.iEliminado_fl = 2;
                    RectorUniversidadPublica.sCreado_by = FrontUser.Get().EmailUtepsa;
                    RectorUniversidadPublica.iConcurrencia_id += 1;

                    db.Entry(RectorUniversidadPublica).State = EntityState.Modified;
                    db.SaveChanges();

                    db.RectoresUniversidadesPublicas.Remove(RectorUniversidadPublica);
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

        private void CargarViewBags(Tarea tarea, gatbl_RectorUniversidadPublica rectorUniversidadPublica)
        {
            ViewBag.unv_codigo = new SelectList(db.Universidades, "unv_codigo", "unv_descripcion", rectorUniversidadPublica.unv_codigo);
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
