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
    public class TutorController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_tribunalTutor_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var tutores = db.Tutorez.Where(p => p.iEliminado_fl == 1).Include(g => g.ColegioProfesional).Include(g => g.Profesion).ToList();
            var tutoresFiltradas = tutores.Where(t => criterio == null || 
                                   t.sNombre_desc.ToLower().Contains(criterio.ToLower()) ||
                                   t.sTelefonos_desc.ToLower().Contains(criterio.ToLower()) ||
                                   t.sDireccion_desc.ToLower().Contains(criterio.ToLower()) ||
                                   t.TipoTutor.ToString().ToLower().Contains(criterio.ToLower()) ||
                                   t.sObs_fl.ToString().ToLower().Contains(criterio.ToLower())
                                   ).ToList();
            var tutoresFiltradasOrdenadas = tutoresFiltradas.OrderBy(ef => ef.sNombre_desc);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + tutoresFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", tutoresFiltradasOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + tutoresFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(tutoresFiltradasOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_tribunalTutor_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Tutores Tutor = db.Tutorez.Find(id);
            if (Tutor == null)
            {
                return HttpNotFound();
            }
            return View(Tutor);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_tribunalTutor_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewBag.iCProfesional_id = new SelectList(db.Profesionales.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "iCProfesional_id", "sNombre_nm");
            ViewBag.iProfesion_id = new SelectList(db.Profesiones.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "iProfesion_id", "sNombre_nm");

            gatbl_Tutores Tutor = new gatbl_Tutores();
            return View(Tutor);
        }

        //CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iTutuor_id,TipoTutor,sNombre_desc,sDireccion_desc,sTelefonos_desc,sObs_fl,sObservacion_desc,EspecialidadTutor,iProfesion_id,iCProfesional_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Tutores Tutor)
        {            
            try
            {
                Tutor.iEstado_fl = true;
                Tutor.iEliminado_fl = 1;
                Tutor.sCreado_by = FrontUser.Get().EmailUtepsa;
                Tutor.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Tutorez.Add(Tutor);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tutor);
                return View(Tutor);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_tribunalTutor_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Tutores Tutor = db.Tutorez.Find(id);
            if (Tutor == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tutor);
            return View(Tutor);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iTutuor_id,TipoTutor,sNombre_desc,sDireccion_desc,sTelefonos_desc,sObs_fl,sObservacion_desc,EspecialidadTutor,iProfesion_id,iCProfesional_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Tutores Tutor)
        {  
            try
            {
                Tutor.iEliminado_fl = 1;
                Tutor.sCreado_by = FrontUser.Get().EmailUtepsa;
                Tutor.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(Tutor).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tutor);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(Tutor);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_tribunalTutor_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Tutores gatbl_Tutores = db.Tutorez.Find(id);
            if (gatbl_Tutores == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Tutores);
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
                    gatbl_Tutores Tutor = db.Tutorez.Find(id);
                    Tutor.iEstado_fl = false;
                    Tutor.iEliminado_fl = 2;
                    Tutor.sCreado_by = FrontUser.Get().EmailUtepsa;
                    Tutor.iConcurrencia_id += 1;

                    db.Entry(Tutor).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Tutorez.Remove(Tutor);
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
      
        //CARGAR VIEW BAGS
        private void CargarViewBags(gatbl_Tutores gatbl_Tutores)
        {
            ViewBag.iCProfesional_id = new SelectList(db.Profesionales.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "iCProfesional_id", "sNombre_nm", gatbl_Tutores.iCProfesional_id);
            ViewBag.iProfesion_id = new SelectList(db.Profesiones.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "iProfesion_id", "sNombre_nm", gatbl_Tutores.iProfesion_id);
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
