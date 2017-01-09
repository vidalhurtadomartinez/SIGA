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
    public class tblinstitucionesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblinstituciones
        public ActionResult Index()
        {
            return View(db.tblinstitucion.ToList());
        }

        // GET: tblinstituciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblinstitucion tblinstitucion = db.tblinstitucion.Find(id);
            if (tblinstitucion == null)
            {
                return HttpNotFound();
            }
            return View(tblinstitucion);
        }

        // GET: tblinstituciones/Create
        public ActionResult Create()
        {
            return View(new tblinstitucion(){ Activo =true});
        }

        // POST: tblinstituciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lInstitucion_id,Descripcion,Activo")] tblinstitucion tblinstitucion)
        {
            if (ModelState.IsValid)
            {
                db.tblinstitucion.Add(tblinstitucion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblinstitucion);
        }

        // GET: tblinstituciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblinstitucion tblinstitucion = db.tblinstitucion.Find(id);
            if (tblinstitucion == null)
            {
                return HttpNotFound();
            }
            return View(tblinstitucion);
        }

        // POST: tblinstituciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lInstitucion_id,Descripcion,Activo")] tblinstitucion tblinstitucion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblinstitucion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblinstitucion);
        }

        // GET: tblinstituciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblinstitucion tblinstitucion = db.tblinstitucion.Find(id);
            if (tblinstitucion == null)
            {
                return HttpNotFound();
            }
            return View(tblinstitucion);
        }

        // POST: tblinstituciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblinstitucion tblinstitucion = db.tblinstitucion.Find(id);
            db.tblinstitucion.Remove(tblinstitucion);
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
