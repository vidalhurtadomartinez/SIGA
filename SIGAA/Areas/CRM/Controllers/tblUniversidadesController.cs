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
    public class tblUniversidadesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblUniversidades
        public ActionResult Index()
        {
            return View(db.tblUniversidad.Include(u=>u.tblTipoUniversidad).Include(u => u.tblCiudad).ToList());
        }

        // GET: tblUniversidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUniversidad tblUniversidad = db.tblUniversidad.Find(id);
            if (tblUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(tblUniversidad);
        }

        // GET: tblUniversidades/Create
        public ActionResult Create()
        {
            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoUniversidad, "lTipoColegio_id", "Descripcion");
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion");

            return View(new tblUniversidad() { Activo = true});
        }

        // POST: tblUniversidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lUniversidad,lTipoUniversidad_id,lCiudad_id,Descripcion,Activo")] tblUniversidad tblUniversidad)
        {
            if (ModelState.IsValid)
            {
                db.tblUniversidad.Add(tblUniversidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.lTipoUniversidad_id = new SelectList(db.tblTipoUniversidad, "lTipoUniversidad_id", "Descripcion", tblUniversidad.lTipoUniversidad_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblUniversidad.lCiudad_id);

            return View(tblUniversidad);
        }

        // GET: tblUniversidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUniversidad tblUniversidad = db.tblUniversidad.Find(id);
            if (tblUniversidad == null)
            {
                return HttpNotFound();
            }

            ViewBag.lTipoUniversidad_id = new SelectList(db.tblTipoUniversidad, "lTipoUniversidad_id", "Descripcion", tblUniversidad.lTipoUniversidad_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblUniversidad.lCiudad_id);

            return View(tblUniversidad);
        }

        // POST: tblUniversidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lUniversidad_id,lTipoUniversidad_id,lCiudad_id,Descripcion,Activo")] tblUniversidad tblUniversidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblUniversidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lTipoUniversidad_id = new SelectList(db.tblTipoUniversidad, "lTipoUniversidad_id", "Descripcion", tblUniversidad.lTipoUniversidad_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblUniversidad.lCiudad_id);

            return View(tblUniversidad);
        }

        // GET: tblUniversidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUniversidad tblUniversidad = db.tblUniversidad.Find(id);
            if (tblUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(tblUniversidad);
        }

        // POST: tblUniversidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblUniversidad tblUniversidad = db.tblUniversidad.Find(id);
            db.tblUniversidad.Remove(tblUniversidad);
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
