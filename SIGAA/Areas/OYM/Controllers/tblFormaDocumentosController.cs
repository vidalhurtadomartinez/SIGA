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
    public class tblFormaDocumentosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_formaDeDocumento_puedeVerIndice)]
        public ActionResult Index()
        {
            var tblFormaDocumentos = db.tblFormaDocumentos.Include(t => t.tblFormaDocumentoTipos);
            return View(tblFormaDocumentos.ToList());
        }

        [Permiso(Permiso = RolesPermisos.OYM_formaDeDocumento_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaDocumento tblFormaDocumento = db.tblFormaDocumentos.Find(id);
            if (tblFormaDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaDocumento);
        }

        [Permiso(Permiso = RolesPermisos.OYM_formaDeDocumento_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewBag.lFormaDocumentoTipo_id = new SelectList(db.tblFormaDocumentoTipos, "lFormaDocumentoTipo_id", "sDescripcion");
            return View(new tblFormaDocumento() { bActivo = true });            
        }

        // POST: tblFormaDocumentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lFormaDocumento_id,lFormaDocumentoTipo_id,sDescripcion,bActivo")] tblFormaDocumento tblFormaDocumento)
        {
            if (ModelState.IsValid)
            {
                db.tblFormaDocumentos.Add(tblFormaDocumento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lFormaDocumentoTipo_id = new SelectList(db.tblFormaDocumentoTipos, "lFormaDocumentoTipo_id", "sDescripcion", tblFormaDocumento.lFormaDocumentoTipo_id);
            return View(tblFormaDocumento);
        }

        [Permiso(Permiso = RolesPermisos.OYM_formaDeDocumento_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaDocumento tblFormaDocumento = db.tblFormaDocumentos.Find(id);
            if (tblFormaDocumento == null)
            {
                return HttpNotFound();
            }
            ViewBag.lFormaDocumentoTipo_id = new SelectList(db.tblFormaDocumentoTipos, "lFormaDocumentoTipo_id", "sDescripcion", tblFormaDocumento.lFormaDocumentoTipo_id);
            return View(tblFormaDocumento);
        }

        // POST: tblFormaDocumentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lFormaDocumento_id,lFormaDocumentoTipo_id,sDescripcion,bActivo")] tblFormaDocumento tblFormaDocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblFormaDocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lFormaDocumentoTipo_id = new SelectList(db.tblFormaDocumentoTipos, "lFormaDocumentoTipo_id", "sDescripcion", tblFormaDocumento.lFormaDocumentoTipo_id);
            return View(tblFormaDocumento);
        }

        [Permiso(Permiso = RolesPermisos.OYM_formaDeDocumento_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaDocumento tblFormaDocumento = db.tblFormaDocumentos.Find(id);
            if (tblFormaDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaDocumento);
        }

        // POST: tblFormaDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblFormaDocumento tblFormaDocumento = db.tblFormaDocumentos.Find(id);
            db.tblFormaDocumentos.Remove(tblFormaDocumento);
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
