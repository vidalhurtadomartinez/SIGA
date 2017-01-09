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
    public class tblTipoColegiosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblTipoColegios
        public ActionResult Index()
        {
            return View(db.tblTipoColegio.ToList());
        }

        // GET: tblTipoColegios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoColegio tblTipoColegio = db.tblTipoColegio.Find(id);
            if (tblTipoColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoColegio);
        }

        // GET: tblTipoColegios/Create
        public ActionResult Create()
        {
            return View(new tblTipoColegio() { Activo=true});
        }

        // POST: tblTipoColegios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoColegio_id,Descripcion,Activo")] tblTipoColegio tblTipoColegio)
        {
            if (ModelState.IsValid)
            {
                db.tblTipoColegio.Add(tblTipoColegio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblTipoColegio);
        }

        // GET: tblTipoColegios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoColegio tblTipoColegio = db.tblTipoColegio.Find(id);
            if (tblTipoColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoColegio);
        }

        // POST: tblTipoColegios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoColegio_id,Descripcion,Activo")] tblTipoColegio tblTipoColegio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTipoColegio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTipoColegio);
        }

        // GET: tblTipoColegios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoColegio tblTipoColegio = db.tblTipoColegio.Find(id);
            if (tblTipoColegio == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoColegio);
        }

        // POST: tblTipoColegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoColegio tblTipoColegio = db.tblTipoColegio.Find(id);
            db.tblTipoColegio.Remove(tblTipoColegio);
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
