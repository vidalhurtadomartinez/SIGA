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
    public class tblCargoAgendasController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblCargoAgendas
        public ActionResult Index()
        {
            var tblCargoAgenda = db.tblCargoAgenda.Include(t => t.agenda).Include(t => t.tblCargo);
            return View(tblCargoAgenda.ToList());
        }

        // GET: tblCargoAgendas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargoAgenda tblCargoAgenda = db.tblCargoAgenda.Find(id);
            if (tblCargoAgenda == null)
            {
                return HttpNotFound();
            }
            return View(tblCargoAgenda);
        }

        // GET: tblCargoAgendas/Create
        public ActionResult Create()
        {
            ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "NombreCompleto");
            ViewBag.lCargo_id = new SelectList(db.tblCargo, "lCargo_id", "sDescripcion");
            return View();
        }

        // POST: tblCargoAgendas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCargoAgenda tblCargoAgenda)
        {
            if (ModelState.IsValid)
            {
                tblCargoAgenda.iEstado_fl = 1;
                tblCargoAgenda.iEliminado_fl = 1;
                tblCargoAgenda.sCreado_by = 1;
                tblCargoAgenda.iConcurrencia_id = 1;

                db.tblCargoAgenda.Add(tblCargoAgenda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "NombreCompleto", tblCargoAgenda.agd_codigo);
            ViewBag.lCargo_id = new SelectList(db.tblCargo, "lCargo_id", "sDescripcion", tblCargoAgenda.lCargo_id);
            return View(tblCargoAgenda);
        }

        // GET: tblCargoAgendas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargoAgenda tblCargoAgenda = db.tblCargoAgenda.Find(id);
            if (tblCargoAgenda == null)
            {
                return HttpNotFound();
            }
            ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "NombreCompleto", tblCargoAgenda.agd_codigo);
            ViewBag.lCargo_id = new SelectList(db.tblCargo, "lCargo_id", "sDescripcion", tblCargoAgenda.lCargo_id);
            return View(tblCargoAgenda);
        }

        // POST: tblCargoAgendas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCargoAgenda tblCargoAgenda)
        {
            if (ModelState.IsValid)
            {
                tblCargoAgenda.iEstado_fl = 1;
                tblCargoAgenda.iEliminado_fl = 1;
                tblCargoAgenda.sCreado_by = 1;
                tblCargoAgenda.iConcurrencia_id = 1;

                db.Entry(tblCargoAgenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "NombreCompleto", tblCargoAgenda.agd_codigo);
            ViewBag.lCargo_id = new SelectList(db.tblCargo, "lCargo_id", "sDescripcion", tblCargoAgenda.lCargo_id);
            return View(tblCargoAgenda);
        }

        // GET: tblCargoAgendas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCargoAgenda tblCargoAgenda = db.tblCargoAgenda.Find(id);
            if (tblCargoAgenda == null)
            {
                return HttpNotFound();
            }
            return View(tblCargoAgenda);
        }

        // POST: tblCargoAgendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCargoAgenda tblCargoAgenda = db.tblCargoAgenda.Find(id);
            db.tblCargoAgenda.Remove(tblCargoAgenda);
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
