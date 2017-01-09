using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CRM.Models;

namespace SIGAA.Areas.CRM.Controllers
{
    public class tblTipoDocumentosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblTipoDocumentos
        public ActionResult Index()
        {
            return View(db.tblTipoDocumento.ToList());
        }

        // GET: tblTipoDocumentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumento.Find(id);
            if (tblTipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoDocumento);
        }

        // GET: tblTipoDocumentos/Create
        public ActionResult Create()
        {
            return View(new tblTipoDocumento() { Activo = true});
        }

        // POST: tblTipoDocumentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoDocumento_id,Descripcion,Activo")] tblTipoDocumento tblTipoDocumento)
        {
            if (ModelState.IsValid)
            {
                db.tblTipoDocumento.Add(tblTipoDocumento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblTipoDocumento);
        }

        // GET: tblTipoDocumentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumento.Find(id);
            if (tblTipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoDocumento);
        }

        // POST: tblTipoDocumentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoDocumento_id,Descripcion,Activo")] tblTipoDocumento tblTipoDocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTipoDocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTipoDocumento);
        }

        // GET: tblTipoDocumentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumento.Find(id);
            if (tblTipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoDocumento);
        }

        // POST: tblTipoDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumento.Find(id);
            db.tblTipoDocumento.Remove(tblTipoDocumento);
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
