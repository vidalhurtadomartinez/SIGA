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
    public class tblTipoRespuestasController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblTipoRespuestas
        public ActionResult Index()
        {
            return View(db.tblTipoRespuesta.ToList());
        }

        // GET: tblTipoRespuestas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoRespuesta tblTipoRespuesta = db.tblTipoRespuesta.Find(id);
            if (tblTipoRespuesta == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoRespuesta);
        }

        // GET: tblTipoRespuestas/Create
        public ActionResult Create()
        {
            return View(new tblTipoRespuesta() { Activo = true});
        }

        // POST: tblTipoRespuestas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoRespuesta_id,Descripcion,Activo")] tblTipoRespuesta tblTipoRespuesta)
        {
            if (ModelState.IsValid)
            {
                db.tblTipoRespuesta.Add(tblTipoRespuesta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblTipoRespuesta);
        }

        // GET: tblTipoRespuestas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoRespuesta tblTipoRespuesta = db.tblTipoRespuesta.Find(id);
            if (tblTipoRespuesta == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoRespuesta);
        }

        // POST: tblTipoRespuestas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoRespuesta_id,Descripcion,Activo")] tblTipoRespuesta tblTipoRespuesta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblTipoRespuesta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblTipoRespuesta);
        }

        // GET: tblTipoRespuestas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoRespuesta tblTipoRespuesta = db.tblTipoRespuesta.Find(id);
            if (tblTipoRespuesta == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoRespuesta);
        }

        // POST: tblTipoRespuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoRespuesta tblTipoRespuesta = db.tblTipoRespuesta.Find(id);
            db.tblTipoRespuesta.Remove(tblTipoRespuesta);
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
