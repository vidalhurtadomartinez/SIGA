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
    public class tblMedioInformacionesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblMedioInformaciones
        public ActionResult Index()
        {
            return View(db.tblMedioInformacion.ToList());
        }

        // GET: tblMedioInformaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMedioInformacion tblMedioInformacion = db.tblMedioInformacion.Find(id);
            if (tblMedioInformacion == null)
            {
                return HttpNotFound();
            }
            return View(tblMedioInformacion);
        }

        // GET: tblMedioInformaciones/Create
        public ActionResult Create()
        {
            return View(new tblMedioInformacion() { Activo = true});
        }

        // POST: tblMedioInformaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lMedioInformacion_id,Descripcion,Activo")] tblMedioInformacion tblMedioInformacion)
        {
            if (ModelState.IsValid)
            {
                db.tblMedioInformacion.Add(tblMedioInformacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblMedioInformacion);
        }

        // GET: tblMedioInformaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMedioInformacion tblMedioInformacion = db.tblMedioInformacion.Find(id);
            if (tblMedioInformacion == null)
            {
                return HttpNotFound();
            }
            return View(tblMedioInformacion);
        }

        // POST: tblMedioInformaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lMedioInformacion_id,Descripcion,Activo")] tblMedioInformacion tblMedioInformacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMedioInformacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblMedioInformacion);
        }

        // GET: tblMedioInformaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMedioInformacion tblMedioInformacion = db.tblMedioInformacion.Find(id);
            if (tblMedioInformacion == null)
            {
                return HttpNotFound();
            }
            return View(tblMedioInformacion);
        }

        // POST: tblMedioInformaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMedioInformacion tblMedioInformacion = db.tblMedioInformacion.Find(id);
            db.tblMedioInformacion.Remove(tblMedioInformacion);
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
