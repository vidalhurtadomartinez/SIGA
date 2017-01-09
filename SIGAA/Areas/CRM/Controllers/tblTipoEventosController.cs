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
    public class tblTipoEventosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblTipoEventos
        public ActionResult Index()
        {
            return View(db.tblTipoEvento.ToList());
        }

        // GET: tblTipoEventos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoEvento tblTipoEvento = db.tblTipoEvento.Find(id);
            if (tblTipoEvento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoEvento);
        }

        // GET: tblTipoEventos/Create
        public ActionResult Create()
        {
            return View(new tblTipoEvento() { Activo = true});
        }

        // POST: tblTipoEventos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoEvento_id,Descripcion,Activo")] tblTipoEvento tblTipoEvento)
        {
            if (ModelState.IsValid)
            {
                db.tblTipoEvento.Add(tblTipoEvento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblTipoEvento);
        }

        // GET: tblTipoEventos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoEvento tblTipoEvento = db.tblTipoEvento.Find(id);
            if (tblTipoEvento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoEvento);
        }

        // POST: tblTipoEventos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoEvento_id,Descripcion,Activo")] tblTipoEvento tblTipoEvento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTipoEvento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTipoEvento);
        }

        // GET: tblTipoEventos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoEvento tblTipoEvento = db.tblTipoEvento.Find(id);
            if (tblTipoEvento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoEvento);
        }

        // POST: tblTipoEventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoEvento tblTipoEvento = db.tblTipoEvento.Find(id);
            db.tblTipoEvento.Remove(tblTipoEvento);
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
