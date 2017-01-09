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
    public class tblColegiosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblColegios
        public ActionResult Index()
        {
            return View(db.tblColegio.Include(c=> c.tblTipoColegio).Include(c => c.tblCiudad).Include(c => c.tblProvincia).ToList());
        }

        // GET: tblColegios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblColegio tblColegio = db.tblColegio.Find(id);
            if (tblColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblColegio);
        }

        // GET: tblColegios/Create
        public ActionResult Create()
        {
            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion");
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion");
            ViewBag.lProvincia_id = new SelectList(db.tblProvincia, "lProvincia_id", "Descripcion");

            return View(new tblColegio() { Activo = true});
        }

        // POST: tblColegios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lColegio_id,lTipoColegio_id,lCiudad_id,lProvincia_id,Descripcion,Activo")] tblColegio tblColegio)
        {
            if (ModelState.IsValid)
            {
                db.tblColegio.Add(tblColegio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion", tblColegio.lTipoColegio_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblColegio.lCiudad_id);
            ViewBag.lProvincia_id = new SelectList(db.tblProvincia, "lProvincia_id", "Descripcion", tblColegio.lProvincia_id);

            return View(tblColegio);
        }

        // GET: tblColegios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblColegio tblColegio = db.tblColegio.Find(id);
            if (tblColegio == null)
            {
                return HttpNotFound();
            }

            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion", tblColegio.lTipoColegio_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblColegio.lCiudad_id);
            ViewBag.lProvincia_id = new SelectList(db.tblProvincia, "lProvincia_id", "Descripcion", tblColegio.lProvincia_id);

            return View(tblColegio);
        }

        // POST: tblColegios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lColegio_id,lTipoColegio_id,lCiudad_id,lProvincia_id,Descripcion,Activo")] tblColegio tblColegio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblColegio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion", tblColegio.lTipoColegio_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblColegio.lCiudad_id);
            ViewBag.lProvincia_id = new SelectList(db.tblProvincia, "lProvincia_id", "Descripcion", tblColegio.lProvincia_id);

            return View(tblColegio);
        }

        // GET: tblColegios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblColegio tblColegio = db.tblColegio.Find(id);
            if (tblColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblColegio);
        }

        // POST: tblColegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblColegio tblColegio = db.tblColegio.Find(id);
            db.tblColegio.Remove(tblColegio);
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
