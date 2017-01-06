using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
namespace SIGAA.Areas.CONV.Controllers
{
    public class TipoEscalaEvaluacionController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: TipoEscalaEvaluacion
        public ActionResult Index()
        {
            return View(db.TipoEscalaEvaluacions.ToList());
        }

        // GET: TipoEscalaEvaluacion/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEscalaEvaluacion tipoEscalaEvaluacion = db.TipoEscalaEvaluacions.Find(id);
            if (tipoEscalaEvaluacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoEscalaEvaluacion);
        }

        // GET: TipoEscalaEvaluacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoEscalaEvaluacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sTipoEscala_fl,sDescripcion,bActivo")] TipoEscalaEvaluacion tipoEscalaEvaluacion)
        {
            if (ModelState.IsValid)
            {
                db.TipoEscalaEvaluacions.Add(tipoEscalaEvaluacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoEscalaEvaluacion);
        }

        // GET: TipoEscalaEvaluacion/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEscalaEvaluacion tipoEscalaEvaluacion = db.TipoEscalaEvaluacions.Find(id);
            if (tipoEscalaEvaluacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoEscalaEvaluacion);
        }

        // POST: TipoEscalaEvaluacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sTipoEscala_fl,sDescripcion,bActivo")] TipoEscalaEvaluacion tipoEscalaEvaluacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEscalaEvaluacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoEscalaEvaluacion);
        }

        // GET: TipoEscalaEvaluacion/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEscalaEvaluacion tipoEscalaEvaluacion = db.TipoEscalaEvaluacions.Find(id);
            if (tipoEscalaEvaluacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoEscalaEvaluacion);
        }

        // POST: TipoEscalaEvaluacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipoEscalaEvaluacion tipoEscalaEvaluacion = db.TipoEscalaEvaluacions.Find(id);
            db.TipoEscalaEvaluacions.Remove(tipoEscalaEvaluacion);
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
