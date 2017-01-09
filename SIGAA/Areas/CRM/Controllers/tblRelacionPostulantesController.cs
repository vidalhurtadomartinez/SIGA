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
    public class tblRelacionPostulantesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblRelacionPostulantes
        public ActionResult Index()
        {
            return View(db.tblRelacionPostulante.ToList());
        }

        // GET: tblRelacionPostulantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRelacionPostulante tblRelacionPostulante = db.tblRelacionPostulante.Find(id);
            if (tblRelacionPostulante == null)
            {
                return HttpNotFound();
            }
            return View(tblRelacionPostulante);
        }

        // GET: tblRelacionPostulantes/Create
        public ActionResult Create()
        {
            return View(new tblRelacionPostulante() { Activo = true});
        }

        // POST: tblRelacionPostulantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lRelacionPostulante_id,Descripcion,Activo")] tblRelacionPostulante tblRelacionPostulante)
        {
            if (ModelState.IsValid)
            {
                db.tblRelacionPostulante.Add(tblRelacionPostulante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblRelacionPostulante);
        }

        // GET: tblRelacionPostulantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRelacionPostulante tblRelacionPostulante = db.tblRelacionPostulante.Find(id);
            if (tblRelacionPostulante == null)
            {
                return HttpNotFound();
            }
            return View(tblRelacionPostulante);
        }

        // POST: tblRelacionPostulantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lRelacionPostulante_id,Descripcion,Activo")] tblRelacionPostulante tblRelacionPostulante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblRelacionPostulante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblRelacionPostulante);
        }

        // GET: tblRelacionPostulantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblRelacionPostulante tblRelacionPostulante = db.tblRelacionPostulante.Find(id);
            if (tblRelacionPostulante == null)
            {
                return HttpNotFound();
            }
            return View(tblRelacionPostulante);
        }

        // POST: tblRelacionPostulantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblRelacionPostulante tblRelacionPostulante = db.tblRelacionPostulante.Find(id);
            db.tblRelacionPostulante.Remove(tblRelacionPostulante);
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
