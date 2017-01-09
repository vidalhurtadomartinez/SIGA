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
    public class tblProvinciasController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblProvincias
        public ActionResult Index()
        {
            return View(db.tblProvincia.ToList());
        }

        // GET: tblProvincias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProvincia tblProvincia = db.tblProvincia.Find(id);
            if (tblProvincia == null)
            {
                return HttpNotFound();
            }
            return View(tblProvincia);
        }

        // GET: tblProvincias/Create
        public ActionResult Create()
        {
            return View(new tblProvincia() { Activo = true});
        }

        // POST: tblProvincias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lProvincia_id,Descripcion,Activo")] tblProvincia tblProvincia)
        {
            if (ModelState.IsValid)
            {
                db.tblProvincia.Add(tblProvincia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblProvincia);
        }

        // GET: tblProvincias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProvincia tblProvincia = db.tblProvincia.Find(id);
            if (tblProvincia == null)
            {
                return HttpNotFound();
            }
            return View(tblProvincia);
        }

        // POST: tblProvincias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lProvincia_id,Descripcion,Activo")] tblProvincia tblProvincia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblProvincia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblProvincia);
        }

        // GET: tblProvincias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProvincia tblProvincia = db.tblProvincia.Find(id);
            if (tblProvincia == null)
            {
                return HttpNotFound();
            }
            return View(tblProvincia);
        }

        // POST: tblProvincias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblProvincia tblProvincia = db.tblProvincia.Find(id);
            db.tblProvincia.Remove(tblProvincia);
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
