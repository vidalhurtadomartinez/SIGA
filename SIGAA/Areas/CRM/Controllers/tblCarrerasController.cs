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
    public class tblCarrerasController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblCarreras
        public ActionResult Index()
        {
            return View(db.tblCarrera.Include(c=> c.tblUniversidad).ToList());
        }

        // GET: tblCarreras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCarrera tblCarrera = db.tblCarrera.Find(id);
            if (tblCarrera == null)
            {
                return HttpNotFound();
            }
            return View(tblCarrera);
        }

        // GET: tblCarreras/Create
        public ActionResult Create()
        {
            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion");

            return View(new tblCarrera() { Activo = true});
        }

        // POST: tblCarreras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lCarrera_id,lUniversidad_id,Descripcion,Activo")] tblCarrera tblCarrera)
        {
            if (ModelState.IsValid)
            {
                db.tblCarrera.Add(tblCarrera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion", tblCarrera.lUniversidad_id);

            return View(tblCarrera);
        }

        // GET: tblCarreras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCarrera tblCarrera = db.tblCarrera.Find(id);
            if (tblCarrera == null)
            {
                return HttpNotFound();
            }

            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion", tblCarrera.lUniversidad_id);

            return View(tblCarrera);
        }

        // POST: tblCarreras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lCarrera_id,lUniversidad_id,Descripcion,Activo")] tblCarrera tblCarrera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCarrera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion", tblCarrera.lUniversidad_id);

            return View(tblCarrera);
        }

        // GET: tblCarreras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCarrera tblCarrera = db.tblCarrera.Find(id);
            if (tblCarrera == null)
            {
                return HttpNotFound();
            }
            return View(tblCarrera);
        }

        // POST: tblCarreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCarrera tblCarrera = db.tblCarrera.Find(id);
            db.tblCarrera.Remove(tblCarrera);
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
