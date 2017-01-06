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
    public class OrigenOtraUniversidadesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: OrigenOtraUniversidades
        public ActionResult Index()
        {
            return View(db.OrigenOtraUniversidades.ToList());
        }

        // GET: OrigenOtraUniversidades/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrigenOtraUniversidad origenOtraUniversidad = db.OrigenOtraUniversidades.Find(id);
            if (origenOtraUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(origenOtraUniversidad);
        }

        // GET: OrigenOtraUniversidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrigenOtraUniversidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sOrigen_fl,sDescripcion,bActivo")] OrigenOtraUniversidad origenOtraUniversidad)
        {
            if (ModelState.IsValid)
            {
                db.OrigenOtraUniversidades.Add(origenOtraUniversidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(origenOtraUniversidad);
        }

        // GET: OrigenOtraUniversidades/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrigenOtraUniversidad origenOtraUniversidad = db.OrigenOtraUniversidades.Find(id);
            if (origenOtraUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(origenOtraUniversidad);
        }

        // POST: OrigenOtraUniversidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sOrigen_fl,sDescripcion,bActivo")] OrigenOtraUniversidad origenOtraUniversidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(origenOtraUniversidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(origenOtraUniversidad);
        }

        // GET: OrigenOtraUniversidades/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrigenOtraUniversidad origenOtraUniversidad = db.OrigenOtraUniversidades.Find(id);
            if (origenOtraUniversidad == null)
            {
                return HttpNotFound();
            }
            return View(origenOtraUniversidad);
        }

        // POST: OrigenOtraUniversidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrigenOtraUniversidad origenOtraUniversidad = db.OrigenOtraUniversidades.Find(id);
            db.OrigenOtraUniversidades.Remove(origenOtraUniversidad);
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
