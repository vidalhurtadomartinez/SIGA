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
    public class tblRubroActividadesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblRubroActividades
        public ActionResult Index()
        {
            return View(db.tblRubroActividad.ToList());
        }

        // GET: tblRubroActividades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRubroActividad tblRubroActividad = db.tblRubroActividad.Find(id);
            if (tblRubroActividad == null)
            {
                return HttpNotFound();
            }
            return View(tblRubroActividad);
        }

        // GET: tblRubroActividades/Create
        public ActionResult Create()
        {
            return View(new tblRubroActividad() { Activo = true});
        }

        // POST: tblRubroActividades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lRubroActividad_id,Descripcion,Activo")] tblRubroActividad tblRubroActividad)
        {
            if (ModelState.IsValid)
            {
                db.tblRubroActividad.Add(tblRubroActividad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblRubroActividad);
        }

        // GET: tblRubroActividades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRubroActividad tblRubroActividad = db.tblRubroActividad.Find(id);
            if (tblRubroActividad == null)
            {
                return HttpNotFound();
            }
            return View(tblRubroActividad);
        }

        // POST: tblRubroActividades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lRubroActividad_id,Descripcion,Activo")] tblRubroActividad tblRubroActividad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRubroActividad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblRubroActividad);
        }

        // GET: tblRubroActividades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRubroActividad tblRubroActividad = db.tblRubroActividad.Find(id);
            if (tblRubroActividad == null)
            {
                return HttpNotFound();
            }
            return View(tblRubroActividad);
        }

        // POST: tblRubroActividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRubroActividad tblRubroActividad = db.tblRubroActividad.Find(id);
            db.tblRubroActividad.Remove(tblRubroActividad);
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
