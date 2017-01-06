using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace SIGAA.Areas.CONV.Controllers
{
    public class PensumsController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: Pensums
        public ActionResult Index()
        {
            var pensums = db.Pensums.Include(p => p.gatbl_Carreras).Include(p => p.gatbl_Universidades);
            return View(pensums.ToList());
        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var pens = from p in db.Pensums.Include(p => p.gatbl_Carreras).Include(p => p.gatbl_Universidades)
                       select p;
            return Json(pens.ToDataSourceResult(request, p => new
            {
                p.lPensum_id,
                p.sDescripcion,
                p.lUniversidad_id,
                p.lCarrera_id,
                p.bActivo,
                gatbl_Universidades = new
                {
                    p.lUniversidad_id,
                    p.gatbl_Universidades.sNombre_desc,
                    p.gatbl_Universidades.sDireccion_desc
                },
                gatbl_Carreras = new
                {
                    p.lCarrera_id,
                    p.gatbl_Carreras.sCarrera_nm,
                    p.gatbl_Carreras.sResponsable_nm
                }
            }));
        }

        public JsonResult CarreraList(int id)
        {
            var carreras = from s in db.gatbl_Carreras
                             where s.lUniversidad_id == id
                             select s;

            return Json(new SelectList(carreras.ToArray(), "lCarrera_id", "sCarrera_nm"), JsonRequestBehavior.AllowGet);
        }

        // GET: Pensums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pensum pensum = db.Pensums.Find(id);
            if (pensum == null)
            {
                return HttpNotFound();
            }
            return View(pensum);
        }

        // GET: Pensums/Create
        public ActionResult Create()
        {
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            return View();
        }

        // POST: Pensums/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pensum pensum)
        {
            if (ModelState.IsValid)
            {
                pensum.iEstado_fl = "1";
                pensum.iEliminado_fl = "1";
                pensum.sCreated_by = DateTime.Now.ToString();
                pensum.iConcurrencia_id = 1;

                db.Pensums.Add(pensum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", pensum.lCarrera_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", pensum.lUniversidad_id);
            ViewBag.UniversidadList = db.gatbl_Universidades;
            return View(pensum);
        }

        // GET: Pensums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pensum pensum = db.Pensums.Find(id);
            if (pensum == null)
            {
                return HttpNotFound();
            }
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c=> c.lUniversidad_id == pensum.lUniversidad_id), "lCarrera_id", "sCarrera_nm", pensum.lCarrera_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", pensum.lUniversidad_id);
            return View(pensum);
        }

        // POST: Pensums/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pensum pensum)
        {
            if (ModelState.IsValid)
            {
                pensum.iEstado_fl = "1";
                pensum.iEliminado_fl = "1";
                pensum.sCreated_by = DateTime.Now.ToString();
                pensum.iConcurrencia_id = 1;

                db.Entry(pensum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == pensum.lUniversidad_id), "lCarrera_id", "sCarrera_nm", pensum.lCarrera_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", pensum.lUniversidad_id);
            return View(pensum);
        }

        // GET: Pensums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pensum pensum = db.Pensums.Find(id);
            if (pensum == null)
            {
                return HttpNotFound();
            }
            return View(pensum);
        }

        // POST: Pensums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pensum pensum = db.Pensums.Find(id);
            db.Pensums.Remove(pensum);
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
