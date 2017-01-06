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
    public class OrigenProgramaAnaliticosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: OrigenProgramaAnaliticos
        public ActionResult Index()
        {
            return View(db.OrigenProgramaAnaliticos.ToList());
        }

        // GET: OrigenProgramaAnaliticos/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrigenProgramaAnalitico origenProgramaAnalitico = db.OrigenProgramaAnaliticos.Find(id);
            if (origenProgramaAnalitico == null)
            {
                return HttpNotFound();
            }
            return View(origenProgramaAnalitico);
        }

        // GET: OrigenProgramaAnaliticos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrigenProgramaAnaliticos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sOrigen_fl,sDescripcion,bActivo")] OrigenProgramaAnalitico origenProgramaAnalitico)
        {
            if (ModelState.IsValid)
            {
                db.OrigenProgramaAnaliticos.Add(origenProgramaAnalitico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(origenProgramaAnalitico);
        }

        // GET: OrigenProgramaAnaliticos/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrigenProgramaAnalitico origenProgramaAnalitico = db.OrigenProgramaAnaliticos.Find(id);
            if (origenProgramaAnalitico == null)
            {
                return HttpNotFound();
            }
            return View(origenProgramaAnalitico);
        }

        // POST: OrigenProgramaAnaliticos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sOrigen_fl,sDescripcion,bActivo")] OrigenProgramaAnalitico origenProgramaAnalitico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(origenProgramaAnalitico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(origenProgramaAnalitico);
        }

        // GET: OrigenProgramaAnaliticos/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrigenProgramaAnalitico origenProgramaAnalitico = db.OrigenProgramaAnaliticos.Find(id);
            if (origenProgramaAnalitico == null)
            {
                return HttpNotFound();
            }
            return View(origenProgramaAnalitico);
        }

        // POST: OrigenProgramaAnaliticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrigenProgramaAnalitico origenProgramaAnalitico = db.OrigenProgramaAnaliticos.Find(id);
            db.OrigenProgramaAnaliticos.Remove(origenProgramaAnalitico);
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
