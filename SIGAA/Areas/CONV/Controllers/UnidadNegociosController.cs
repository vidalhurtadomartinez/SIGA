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
    public class UnidadNegociosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: UnidadNegocios
        public ActionResult Index()
        {
            return View(db.UnidadNegocios.ToList());
        }

        // GET: UnidadNegocios/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            if (unidadNegocio == null)
            {
                return HttpNotFound();
            }
            return View(unidadNegocio);
        }

        // GET: UnidadNegocios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadNegocios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lUNegocio_id,sDescripcion,bActivo")] UnidadNegocio unidadNegocio)
        {
            if (ModelState.IsValid)
            {
                db.UnidadNegocios.Add(unidadNegocio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unidadNegocio);
        }

        // GET: UnidadNegocios/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            if (unidadNegocio == null)
            {
                return HttpNotFound();
            }
            return View(unidadNegocio);
        }

        // POST: UnidadNegocios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lUNegocio_id,sDescripcion,bActivo")] UnidadNegocio unidadNegocio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidadNegocio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidadNegocio);
        }

        // GET: UnidadNegocios/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            if (unidadNegocio == null)
            {
                return HttpNotFound();
            }
            return View(unidadNegocio);
        }

        // POST: UnidadNegocios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            db.UnidadNegocios.Remove(unidadNegocio);
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
