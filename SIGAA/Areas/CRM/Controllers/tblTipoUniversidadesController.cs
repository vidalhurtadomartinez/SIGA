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
    public class tblTipoUniversidadesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblTipoUniversidades
        public ActionResult Index()
        {
            return View(db.tblTipoUniversidad.ToList());
        }

        // GET: tblTipoUniversidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoUniversidad tblTipoUniversidad = db.tblTipoUniversidad.Find(id);
            if (tblTipoUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoUniversidad);
        }

        // GET: tblTipoUniversidades/Create
        public ActionResult Create()
        {
            return View(new tblTipoUniversidad() { Activo = true});
        }

        // POST: tblTipoUniversidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoUniversidad_id,Descripcion,Activo")] tblTipoUniversidad tblTipoUniversidad)
        {
            if (ModelState.IsValid)
            {
                db.tblTipoUniversidad.Add(tblTipoUniversidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblTipoUniversidad);
        }

        // GET: tblTipoUniversidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoUniversidad tblTipoUniversidad = db.tblTipoUniversidad.Find(id);
            if (tblTipoUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoUniversidad);
        }

        // POST: tblTipoUniversidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoUniversidad_id,Descripcion,Activo")] tblTipoUniversidad tblTipoUniversidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTipoUniversidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTipoUniversidad);
        }

        // GET: tblTipoUniversidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoUniversidad tblTipoUniversidad = db.tblTipoUniversidad.Find(id);
            if (tblTipoUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoUniversidad);
        }

        // POST: tblTipoUniversidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoUniversidad tblTipoUniversidad = db.tblTipoUniversidad.Find(id);
            db.tblTipoUniversidad.Remove(tblTipoUniversidad);
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
