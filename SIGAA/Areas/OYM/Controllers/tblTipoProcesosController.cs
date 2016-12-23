using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblTipoProcesosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeProcesos_puedeVerIndice)]
        public ActionResult Index()
        {
            return View(db.tblTipoProcesos.ToList());
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeProcesos_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoProceso tblTipoProceso = db.tblTipoProcesos.Find(id);
            if (tblTipoProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeProcesos_puedeCrearNuevo)]
        public ActionResult Create()
        {
            return View(new tblTipoProceso() { bActivo = true });
        }

        // POST: tblTipoProcesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoProceso_id,sSigla,sDescripcion,bActivo")] tblTipoProceso tblTipoProceso)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = db.tblTipoProcesos.Where(t => t.sSigla == tblTipoProceso.sSigla).Count() > 0;

                if (isDuplicate)
                {
                    ViewBag.Error = "La sigla no puede ser duplicada";

                    return View(tblTipoProceso);
                }
                else
                {
                    db.tblTipoProcesos.Add(tblTipoProceso);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }

            return View(tblTipoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeProcesos_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoProceso tblTipoProceso = db.tblTipoProcesos.Find(id);
            if (tblTipoProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoProceso);
        }

        // POST: tblTipoProcesos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoProceso_id,sSigla,sDescripcion,bActivo")] tblTipoProceso tblTipoProceso)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = db.tblTipoProcesos.Where(t => t.sSigla == tblTipoProceso.sSigla && t.lTipoProceso_id != tblTipoProceso.lTipoProceso_id).Count() > 0;

                if (isDuplicate)
                {
                    ViewBag.Error = "La sigla no puede ser duplicada";

                    return View(tblTipoProceso);
                }
                else
                {
                    db.Entry(tblTipoProceso).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }
            return View(tblTipoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeProcesos_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoProceso tblTipoProceso = db.tblTipoProcesos.Find(id);
            if (tblTipoProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoProceso);
        }

        // POST: tblTipoProcesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoProceso tblTipoProceso = db.tblTipoProcesos.Find(id);
            db.tblTipoProcesos.Remove(tblTipoProceso);
            db.SaveChanges();
            return RedirectToAction("Index");
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
