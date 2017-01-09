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
    public class tblAsesoresController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblAsesores
        public ActionResult Index()
        {
            return View(db.tblAsesor.ToList());
        }

        // GET: tblAsesores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAsesor tblAsesor = db.tblAsesor.Find(id);
            if (tblAsesor == null)
            {
                return HttpNotFound();
            }
            return View(tblAsesor);
        }

        // GET: tblAsesores/Create
        public ActionResult Create()
        {
            return View(new tblAsesor() { dtFechaRegistro_dt= DateTime.Now});
        }

        // POST: tblAsesores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAsesor tblAsesor)
        {
            if (ModelState.IsValid)
            {
                tblAsesor.dtFechaRegistro_dt = DateTime.Now;
                tblAsesor.iEstado_fl = "01";
                tblAsesor.iEliminado_fl = "01";
                tblAsesor.sCreated_by = DateTime.Now.ToString();
                tblAsesor.iConcurrencia_id = 1;
               

                db.tblAsesor.Add(tblAsesor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblAsesor);
        }

        // GET: tblAsesores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAsesor tblAsesor = db.tblAsesor.Find(id);
            if (tblAsesor == null)
            {
                return HttpNotFound();
            }
            return View(tblAsesor);
        }

        // POST: tblAsesores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAsesor tblAsesor)
        {
            if (ModelState.IsValid)
            {
                var asesor = db.tblAsesor.Find(tblAsesor.lAsesor_id);
                asesor.Nombres = tblAsesor.Nombres;
                asesor.ApellidoPaterno = tblAsesor.ApellidoPaterno;
                asesor.ApellidoMaterno = tblAsesor.ApellidoMaterno;
                asesor.Direccion = tblAsesor.Direccion;
                asesor.Telefono = tblAsesor.Telefono;
                asesor.Celular = tblAsesor.Celular;
                asesor.Comision = tblAsesor.Comision;

                db.Entry(asesor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblAsesor);
        }

        // GET: tblAsesores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAsesor tblAsesor = db.tblAsesor.Find(id);
            if (tblAsesor == null)
            {
                return HttpNotFound();
            }
            return View(tblAsesor);
        }

        // POST: tblAsesores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAsesor tblAsesor = db.tblAsesor.Find(id);
            db.tblAsesor.Remove(tblAsesor);
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
