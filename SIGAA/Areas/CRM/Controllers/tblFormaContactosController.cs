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
    public class tblFormaContactosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblFormaContactos
        public ActionResult Index()
        {
            return View(db.tblFormaContacto.ToList());
        }

        // GET: tblFormaContactos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaContacto tblFormaContacto = db.tblFormaContacto.Find(id);
            if (tblFormaContacto == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaContacto);
        }

        // GET: tblFormaContactos/Create
        public ActionResult Create()
        {
            return View(new tblFormaContacto() { Activo = true});
        }

        // POST: tblFormaContactos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lFormaContacto_id,Descripcion,Activo")] tblFormaContacto tblFormaContacto)
        {
            if (ModelState.IsValid)
            {
                db.tblFormaContacto.Add(tblFormaContacto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblFormaContacto);
        }

        // GET: tblFormaContactos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaContacto tblFormaContacto = db.tblFormaContacto.Find(id);
            if (tblFormaContacto == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaContacto);
        }

        // POST: tblFormaContactos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lFormaContacto_id,Descripcion,Activo")] tblFormaContacto tblFormaContacto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblFormaContacto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblFormaContacto);
        }

        // GET: tblFormaContactos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormaContacto tblFormaContacto = db.tblFormaContacto.Find(id);
            if (tblFormaContacto == null)
            {
                return HttpNotFound();
            }
            return View(tblFormaContacto);
        }

        // POST: tblFormaContactos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblFormaContacto tblFormaContacto = db.tblFormaContacto.Find(id);
            db.tblFormaContacto.Remove(tblFormaContacto);
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
