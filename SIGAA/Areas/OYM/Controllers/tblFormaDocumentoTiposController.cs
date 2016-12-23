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
    public class tblFormaDocumentoTiposController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeFormasDeDocumento_puedeVerIndice)]
        public ActionResult Index()
        {
            return View(db.tblFormaDocumentoTipos.ToList());
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeFormasDeDocumento_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaDocumentoTipo tblFormaDocumentoTipo = db.tblFormaDocumentoTipos.Find(id);
            if (tblFormaDocumentoTipo == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaDocumentoTipo);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeFormasDeDocumento_puedeCrearNuevo)]
        public ActionResult Create()
        {
            return View(new tblFormaDocumentoTipo { bActivo = true});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lFormaDocumentoTipo_id,sDescripcion,bActivo")] tblFormaDocumentoTipo tblFormaDocumentoTipo)
        {
            if (ModelState.IsValid)
            {
                db.tblFormaDocumentoTipos.Add(tblFormaDocumentoTipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblFormaDocumentoTipo);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeFormasDeDocumento_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaDocumentoTipo tblFormaDocumentoTipo = db.tblFormaDocumentoTipos.Find(id);
            if (tblFormaDocumentoTipo == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaDocumentoTipo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lFormaDocumentoTipo_id,sDescripcion,bActivo")] tblFormaDocumentoTipo tblFormaDocumentoTipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblFormaDocumentoTipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblFormaDocumentoTipo);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeFormasDeDocumento_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaDocumentoTipo tblFormaDocumentoTipo = db.tblFormaDocumentoTipos.Find(id);
            if (tblFormaDocumentoTipo == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaDocumentoTipo);
        }

        // POST: tblFormaDocumentoTipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblFormaDocumentoTipo tblFormaDocumentoTipo = db.tblFormaDocumentoTipos.Find(id);
            db.tblFormaDocumentoTipos.Remove(tblFormaDocumentoTipo);
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
