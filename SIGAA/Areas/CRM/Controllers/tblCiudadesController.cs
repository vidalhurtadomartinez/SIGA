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
    public class tblCiudadesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblCiudades
        public ActionResult Index()
        {
            return View(db.tblCiudad.ToList());
        }

        // GET: tblCiudades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCiudad tblCiudad = db.tblCiudad.Find(id);
            if (tblCiudad == null)
            {
                return HttpNotFound();
            }
            return View(tblCiudad);
        }

        // GET: tblCiudades/Create
        public ActionResult Create()
        {
            return View(new tblCiudad() { Activo = true});
        }

        // POST: tblCiudades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lCiudad_id,Prefijo,Descripcion,Activo")] tblCiudad tblCiudad)
        {
            if (ModelState.IsValid)
            {
                db.tblCiudad.Add(tblCiudad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCiudad);
        }

        // GET: tblCiudades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCiudad tblCiudad = db.tblCiudad.Find(id);
            if (tblCiudad == null)
            {
                return HttpNotFound();
            }
            return View(tblCiudad);
        }

        // POST: tblCiudades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lCiudad_id,Prefijo,Descripcion,Activo")] tblCiudad tblCiudad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCiudad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCiudad);
        }

        // GET: tblCiudades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCiudad tblCiudad = db.tblCiudad.Find(id);
            if (tblCiudad == null)
            {
                return HttpNotFound();
            }
            return View(tblCiudad);
        }

        // POST: tblCiudades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCiudad tblCiudad = db.tblCiudad.Find(id);
            db.tblCiudad.Remove(tblCiudad);
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
