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
    public class tblPaisesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblPaises
        public ActionResult Index()
        {
            return View(db.tblPais.ToList());
        }

        // GET: tblPaises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPais tblPais = db.tblPais.Find(id);
            if (tblPais == null)
            {
                return HttpNotFound();
            }
            return View(tblPais);
        }

        // GET: tblPaises/Create
        public ActionResult Create()
        {
            return View(new tblPais() { Activo = true});
        }

        // POST: tblPaises/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lPais_id,Descripcion,Gentilicio,Activo")] tblPais tblPais)
        {
            if (ModelState.IsValid)
            {
                db.tblPais.Add(tblPais);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPais);
        }

        // GET: tblPaises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPais tblPais = db.tblPais.Find(id);
            if (tblPais == null)
            {
                return HttpNotFound();
            }
            return View(tblPais);
        }

        // POST: tblPaises/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lPais_id,Descripcion,Gentilicio,Activo")] tblPais tblPais)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPais).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPais);
        }

        // GET: tblPaises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPais tblPais = db.tblPais.Find(id);
            if (tblPais == null)
            {
                return HttpNotFound();
            }
            return View(tblPais);
        }

        // POST: tblPaises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPais tblPais = db.tblPais.Find(id);
            db.tblPais.Remove(tblPais);
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
