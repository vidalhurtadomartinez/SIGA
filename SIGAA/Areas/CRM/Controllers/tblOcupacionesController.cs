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
    public class tblOcupacionesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblOcupaciones
        public ActionResult Index()
        {
            return View(db.tblOcupacion.ToList());
        }

        // GET: tblOcupaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOcupacion tblOcupacion = db.tblOcupacion.Find(id);
            if (tblOcupacion == null)
            {
                return HttpNotFound();
            }
            return View(tblOcupacion);
        }

        // GET: tblOcupaciones/Create
        public ActionResult Create()
        {
            return View(new tblOcupacion() { Activo = true});
        }

        // POST: tblOcupaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lOcupacion_id,Descripcion,Activo")] tblOcupacion tblOcupacion)
        {
            if (ModelState.IsValid)
            {
                db.tblOcupacion.Add(tblOcupacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblOcupacion);
        }

        // GET: tblOcupaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOcupacion tblOcupacion = db.tblOcupacion.Find(id);
            if (tblOcupacion == null)
            {
                return HttpNotFound();
            }
            return View(tblOcupacion);
        }

        // POST: tblOcupaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lOcupacion_id,Descripcion,Activo")] tblOcupacion tblOcupacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblOcupacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblOcupacion);
        }

        // GET: tblOcupaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOcupacion tblOcupacion = db.tblOcupacion.Find(id);
            if (tblOcupacion == null)
            {
                return HttpNotFound();
            }
            return View(tblOcupacion);
        }

        // POST: tblOcupaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblOcupacion tblOcupacion = db.tblOcupacion.Find(id);
            db.tblOcupacion.Remove(tblOcupacion);
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
