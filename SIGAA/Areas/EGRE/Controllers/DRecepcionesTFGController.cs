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

namespace SIGAA.Areas.EGRE.Controllers
{
    public class DRecepcionesTFGController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        //INDICE Y BUSQUEDA ASINCRONA
        public ActionResult Index()
        {
            var dRecepcionesTFG = db.DRecepcionesTFG.Where(p => p.iEliminado_fl == 1).Include(g => g.RecepcionTFG);
            return View(dRecepcionesTFG.ToList());
        }

        //DETALLE GET
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DRecepcionesTFG DRecepcionesTFG = db.DRecepcionesTFG.Find(id);
            if (DRecepcionesTFG == null)
            {
                return HttpNotFound();
            }
            return View(DRecepcionesTFG);
        }

        //CREAR GET
        public ActionResult Create()
        {
            ViewBag.iRecepcionTFG_id = new SelectList(db.RecepcionesTFG.Where(p => p.iEliminado_fl == 1), "iRecepcionTFG_id", "iRecepcionTFG_id");

            gatbl_DRecepcionesTFG DRecepcionesTFG = new gatbl_DRecepcionesTFG();
            return View(DRecepcionesTFG);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iDRecepcionTFG_id,iObs_nro,sTipoObs_fl,sObsCorta,sObsDetallada,sSugerencias,iRecepcionTFG_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_DRecepcionesTFG DRecepcionesTFG)
        {            
            try
            {
                DRecepcionesTFG.iEstado_fl = true;
                DRecepcionesTFG.iEliminado_fl = 1;
                DRecepcionesTFG.sCreado_by = FrontUser.Get().usr_login;
                DRecepcionesTFG.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.DRecepcionesTFG.Add(DRecepcionesTFG);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO   ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarBiewBags(DRecepcionesTFG);
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(DRecepcionesTFG);
            }
            catch (Exception ex)
            {                
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " +ex.Message);
                return RedirectToAction("Index");
            }
        }

        //EDITAR GET
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DRecepcionesTFG DRecepcionesTFG = db.DRecepcionesTFG.Find(id);
            if (DRecepcionesTFG == null)
            {
                return HttpNotFound();
            }
            CargarBiewBags(DRecepcionesTFG);
            return View(DRecepcionesTFG);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iDRecepcionTFG_id,iObs_nro,sTipoObs_fl,sObsCorta,sObsDetallada,sSugerencias,iRecepcionTFG_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_DRecepcionesTFG DRecepcionesTFG)
        {            
            try
            {
                DRecepcionesTFG.iEliminado_fl = 1;
                DRecepcionesTFG.sCreado_by = FrontUser.Get().usr_login;
                DRecepcionesTFG.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(DRecepcionesTFG).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO   ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarBiewBags(DRecepcionesTFG);
                return View(DRecepcionesTFG);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        //ELIMINAR GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DRecepcionesTFG DRecepcionesTFG = db.DRecepcionesTFG.Find(id);
            if (DRecepcionesTFG == null)
            {
                return HttpNotFound();
            }
            return View(DRecepcionesTFG);
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
                    gatbl_DRecepcionesTFG DRecepcionesTFG = db.DRecepcionesTFG.Find(id);
                    DRecepcionesTFG.iEstado_fl = false;
                    DRecepcionesTFG.iEliminado_fl = 2;
                    DRecepcionesTFG.sCreado_by = FrontUser.Get().usr_login;
                    DRecepcionesTFG.iConcurrencia_id += 1;

                    db.Entry(DRecepcionesTFG).State = EntityState.Modified;
                    db.SaveChanges();

                    db.DRecepcionesTFG.Remove(DRecepcionesTFG);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO   ", "El dato ha sido Eliminado correctamente.");
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
        
        //GARGA VIEWBAGS
        private void CargarBiewBags(gatbl_DRecepcionesTFG DRecepcionesTFG) {
            ViewBag.iRecepcionTFG_id = new SelectList(db.RecepcionesTFG.Where(p => p.iEliminado_fl == 1), "iRecepcionTFG_id", "iRecepcionTFG_id", DRecepcionesTFG.iRecepcionTFG_id);
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
