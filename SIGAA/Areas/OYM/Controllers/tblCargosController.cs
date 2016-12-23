using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblCargosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblCargos
        public ActionResult Index()
        {
            return View(db.tblCargo.ToList());
        }

        // GET: tblCargos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargo tblCargo = db.tblCargo.Find(id);
            if (tblCargo == null)
            {
                return HttpNotFound();
            }
            return View(tblCargo);
        }

        // GET: tblCargos/Create
        public ActionResult Create()
        {
            return View(new tblCargo { bActivo = true});
        }

        // POST: tblCargos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lCargo_id,sDescripcion,bActivo")] tblCargo tblCargo)
        {
            if (ModelState.IsValid)
            {
                db.tblCargo.Add(tblCargo);
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
            tblCargo tblCargo = db.tblCargo.Find(id);
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
        public ActionResult Edit([Bind(Include = "lCargo_id,sDescripcion,bActivo")] tblCargo tblCargo)
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
            tblCargo tblCargo = db.tblCargo.Find(id);
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
            tblCargo tblCargo = db.tblCargo.Find(id);
            db.tblCargo.Remove(tblCargo);
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
