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
    public class tblNotificacionesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblNotificaciones
        public ActionResult Index()
        {
            return View(db.tblNotificacion.ToList());
        }

        // GET: tblNotificaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblNotificacion tblNotificacion = db.tblNotificacion.Find(id);
            if (tblNotificacion == null)
            {
                return HttpNotFound();
            }
            return View(tblNotificacion);
        }

        // GET: tblNotificaciones/Create
        public ActionResult Create()
        {
            return View(new tblNotificacion() { Activo = true});
        }

        // POST: tblNotificaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lNotificacion_id,TiempoAlerta,Descripcion,Activo")] tblNotificacion tblNotificacion)
        {
            if (ModelState.IsValid)
            {
                db.tblNotificacion.Add(tblNotificacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblNotificacion);
        }

        // GET: tblNotificaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblNotificacion tblNotificacion = db.tblNotificacion.Find(id);
            if (tblNotificacion == null)
            {
                return HttpNotFound();
            }
            return View(tblNotificacion);
        }

        // POST: tblNotificaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lNotificacion_id,TiempoAlerta,Descripcion,Activo")] tblNotificacion tblNotificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblNotificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblNotificacion);
        }

        // GET: tblNotificaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblNotificacion tblNotificacion = db.tblNotificacion.Find(id);
            if (tblNotificacion == null)
            {
                return HttpNotFound();
            }
            return View(tblNotificacion);
        }

        // POST: tblNotificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblNotificacion tblNotificacion = db.tblNotificacion.Find(id);
            db.tblNotificacion.Remove(tblNotificacion);
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
