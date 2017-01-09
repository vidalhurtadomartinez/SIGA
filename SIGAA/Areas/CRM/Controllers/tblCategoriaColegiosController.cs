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
    public class tblCategoriaColegiosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblCategoriaColegios
        public ActionResult Index()
        {
            return View(db.tblCategoriaColegio.ToList());
        }

        // GET: tblCategoriaColegios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategoriaColegio tblCategoriaColegio = db.tblCategoriaColegio.Find(id);
            if (tblCategoriaColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblCategoriaColegio);
        }

        // GET: tblCategoriaColegios/Create
        public ActionResult Create()
        {
            return View(new tblCategoriaColegio() { Activo = true});
        }

        // POST: tblCategoriaColegios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lCategoriaColegio_id,Descripcion,Activo")] tblCategoriaColegio tblCategoriaColegio)
        {
            if (ModelState.IsValid)
            {
                db.tblCategoriaColegio.Add(tblCategoriaColegio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCategoriaColegio);
        }

        // GET: tblCategoriaColegios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategoriaColegio tblCategoriaColegio = db.tblCategoriaColegio.Find(id);
            if (tblCategoriaColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblCategoriaColegio);
        }

        // POST: tblCategoriaColegios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lCategoriaColegio_id,Descripcion,Activo")] tblCategoriaColegio tblCategoriaColegio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCategoriaColegio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCategoriaColegio);
        }

        // GET: tblCategoriaColegios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategoriaColegio tblCategoriaColegio = db.tblCategoriaColegio.Find(id);
            if (tblCategoriaColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblCategoriaColegio);
        }

        // POST: tblCategoriaColegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCategoriaColegio tblCategoriaColegio = db.tblCategoriaColegio.Find(id);
            db.tblCategoriaColegio.Remove(tblCategoriaColegio);
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
