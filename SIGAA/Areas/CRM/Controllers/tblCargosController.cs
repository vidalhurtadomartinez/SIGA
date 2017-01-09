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
    public class tblCargosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblCargos
        public ActionResult Index()
        {
            return View(db.tblCargos.ToList());
        }

        // GET: tblCargos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargo tblCargo = db.tblCargos.Find(id);
            if (tblCargo == null)
            {
                return HttpNotFound();
            }
            return View(tblCargo);
        }

        // GET: tblCargos/Create
        public ActionResult Create()
        {
            return View(new tblCargo() { Activo = true});
        }

        // POST: tblCargos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lCargo_id,Descripcion,Activo")] tblCargo tblCargo)
        {
            if (ModelState.IsValid)
            {
                db.tblCargos.Add(tblCargo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCargo);
        }

        // GET: tblCargos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargo tblCargo = db.tblCargos.Find(id);
            if (tblCargo == null)
            {
                return HttpNotFound();
            }
            return View(tblCargo);
        }

        // POST: tblCargos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lCargo_id,Descripcion,Activo")] tblCargo tblCargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCargo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCargo);
        }

        // GET: tblCargos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargo tblCargo = db.tblCargos.Find(id);
            if (tblCargo == null)
            {
                return HttpNotFound();
            }
            return View(tblCargo);
        }

        // POST: tblCargos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCargo tblCargo = db.tblCargos.Find(id);
            db.tblCargos.Remove(tblCargo);
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
