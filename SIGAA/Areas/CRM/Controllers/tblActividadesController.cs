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
    public class tblActividadesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblActividades
        public ActionResult Index()
        {
            return View(db.tblActividad.Include(a=> a.tblRubroActividad).ToList());
        }

        // GET: tblActividades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblActividad tblActividad = db.tblActividad.Find(id);
            if (tblActividad == null)
            {
                return HttpNotFound();
            }
            return View(tblActividad);
        }

        // GET: tblActividades/Create
        public ActionResult Create()
        {
            ViewBag.lRubroActividad_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion");

            return View(new tblActividad() { Activo = true});
        }

        // POST: tblActividades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lActividad_id,lRubroActividad_id,Descripcion,Activo")] tblActividad tblActividad)
        {
            if (ModelState.IsValid)
            {
                db.tblActividad.Add(tblActividad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lRubroActividad_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion", tblActividad.lRubroActividad_id);

            return View(tblActividad);
        }

        // GET: tblActividades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblActividad tblActividad = db.tblActividad.Find(id);
            if (tblActividad == null)
            {
                return HttpNotFound();
            }

            ViewBag.lRubroActividad_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion",tblActividad.lRubroActividad_id);

            return View(tblActividad);
        }

        // POST: tblActividades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lActividad_id,lRubroActividad_id,Descripcion,Activo")] tblActividad tblActividad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblActividad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lRubroActividad_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion", tblActividad.lRubroActividad_id);

            return View(tblActividad);
        }

        // GET: tblActividades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblActividad tblActividad = db.tblActividad.Find(id);
            if (tblActividad == null)
            {
                return HttpNotFound();
            }
            return View(tblActividad);
        }

        // POST: tblActividades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblActividad tblActividad = db.tblActividad.Find(id);
            db.tblActividad.Remove(tblActividad);
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
