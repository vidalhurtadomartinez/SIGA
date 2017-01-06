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
    public class TipoDocumentoPersonalesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: TipoDocumentoPersonales
        public ActionResult Index()
        {
            return View(db.TipoDocumentoPersonales.ToList());
        }

        // GET: TipoDocumentoPersonales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            if (tipoDocumentoPersonal == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoPersonal);
        }

        // GET: TipoDocumentoPersonales/Create
        public ActionResult Create()
        {
            return View(new TipoDocumentoPersonal() { bActivo = true });            
        }

        // POST: TipoDocumentoPersonales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoDocumentoPersonal_id,sDescripcion,bActivo")] TipoDocumentoPersonal tipoDocumentoPersonal)
        {
            if (ModelState.IsValid)
            {
                db.TipoDocumentoPersonales.Add(tipoDocumentoPersonal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDocumentoPersonal);
        }

        // GET: TipoDocumentoPersonales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            if (tipoDocumentoPersonal == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoPersonal);
        }

        // POST: TipoDocumentoPersonales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoDocumentoPersonal_id,sDescripcion,bActivo")] TipoDocumentoPersonal tipoDocumentoPersonal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDocumentoPersonal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDocumentoPersonal);
        }

        // GET: TipoDocumentoPersonales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            if (tipoDocumentoPersonal == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoPersonal);
        }

        // POST: TipoDocumentoPersonales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            db.TipoDocumentoPersonales.Remove(tipoDocumentoPersonal);
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
