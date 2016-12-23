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
    public class AreaAdministrativaController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_areaAdministrativa_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var areasAdministrativas = db.AreasAdministrativas.Where(p => p.iEliminado_fl == 1).ToList();
            var areasAdministrativasFiltradas = areasAdministrativas.Where(aa => criterio == null || 
                                                                           aa.sNombre_nm.ToLower().Contains(criterio.ToLower()) ||
                                                                           aa.sUbicacion_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                           aa.sTelefono_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                           aa.sCoordinador_nm.ToLower().Contains(criterio.ToLower())
                                                                           ).ToList();
            var areasAdministrativasFiltradasOrdenadas = areasAdministrativasFiltradas.OrderBy(ef => ef.sNombre_nm);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + areasAdministrativasFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", areasAdministrativasFiltradasOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + areasAdministrativasFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(areasAdministrativasFiltradasOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_areaAdministrativa_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AreasAdministrativas gatbl_AreasAdministrativas = db.AreasAdministrativas.Find(id);
            if (gatbl_AreasAdministrativas == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_AreasAdministrativas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_areaAdministrativa_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_AreasAdministrativas areaAdministrativa = new gatbl_AreasAdministrativas();
            return View(areaAdministrativa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iAreaAdministrativa_id,sNombre_nm,sUbicacion_desc,sTelefono_desc,sCoordinador_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_AreasAdministrativas AreaAdministrativa)
        {            
            try
            {
                AreaAdministrativa.iEstado_fl = true;
                AreaAdministrativa.iEliminado_fl = 1;
                AreaAdministrativa.sCreado_by = FrontUser.Get().EmailUtepsa;
                AreaAdministrativa.iConcurrencia_id = 1;
                if (ModelState.IsValid)
                {
                    db.AreasAdministrativas.Add(AreaAdministrativa);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato se ha Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(AreaAdministrativa);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_areaAdministrativa_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AreasAdministrativas AreaAdministrativa = db.AreasAdministrativas.Find(id);
            if (AreaAdministrativa == null)
            {
                return HttpNotFound();
            }
            return View(AreaAdministrativa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iAreaAdministrativa_id,sNombre_nm,sUbicacion_desc,sTelefono_desc,sCoordinador_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_AreasAdministrativas AreaAdministrativa)
        {           
            try
            {
                AreaAdministrativa.iEliminado_fl = 1;
                AreaAdministrativa.sCreado_by = FrontUser.Get().EmailUtepsa;
                AreaAdministrativa.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(AreaAdministrativa).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(AreaAdministrativa);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_areaAdministrativa_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AreasAdministrativas AreaAdministrativa = db.AreasAdministrativas.Find(id);
            if (AreaAdministrativa == null)
            {
                return HttpNotFound();
            }
            return View(AreaAdministrativa);
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
                    gatbl_AreasAdministrativas AreaAdministrativa = db.AreasAdministrativas.Find(id);
                    AreaAdministrativa.iEstado_fl = false;
                    AreaAdministrativa.iEliminado_fl = 2;
                    AreaAdministrativa.sCreado_by = FrontUser.Get().EmailUtepsa;
                    AreaAdministrativa.iConcurrencia_id += 1;

                    db.Entry(AreaAdministrativa).State = EntityState.Modified;
                    db.SaveChanges();

                    db.AreasAdministrativas.Remove(AreaAdministrativa);
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
