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
    public class gatbl_EscalaCalificacionesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_EscalaCalificaciones
        public ActionResult Index()
        {
            var gatbl_EscalaCalificaciones = db.gatbl_EscalaCalificaciones.Include(g => g.gatbl_Universidades).Include(g => g.TipoEscalaEvaluacion);
            return View(gatbl_EscalaCalificaciones.ToList());
        }

        // GET: gatbl_EscalaCalificaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones = db.gatbl_EscalaCalificaciones.Find(id);
            if (gatbl_EscalaCalificaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_EscalaCalificaciones);
        }

        // GET: gatbl_EscalaCalificaciones/Create
        public ActionResult Create()
        {
            ViewBag.lUniversidad_id = db.gatbl_Universidades.Select(g => new { lUniversidad_id = g.lUniversidad_id, sNombre_desc = g.sNombre_desc.Trim() }).OrderBy(g => g.sNombre_desc);
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.sTipoEscala_fl = new SelectList(db.TipoEscalaEvaluacions, "sTipoEscala_fl", "sDescripcion");
            return View();
        }

        // POST: gatbl_EscalaCalificaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones)
        {
            if (ModelState.IsValid)
            {
                gatbl_EscalaCalificaciones.iEstado_fl = "1";
                gatbl_EscalaCalificaciones.iEliminado_fl = "1";
                gatbl_EscalaCalificaciones.sCreated_by = DateTime.Now.ToString();
                gatbl_EscalaCalificaciones.iConcurrencia_id = 1;

                db.gatbl_EscalaCalificaciones.Add(gatbl_EscalaCalificaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lUniversidad_id = db.gatbl_Universidades.Select(g => new { lUniversidad_id = g.lUniversidad_id, sNombre_desc = g.sNombre_desc.Trim() }).OrderBy(g => g.sNombre_desc);
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_EscalaCalificaciones.lUniversidad_id);
            ViewBag.sTipoEscala_fl = new SelectList(db.TipoEscalaEvaluacions, "sTipoEscala_fl", "sDescripcion", gatbl_EscalaCalificaciones.sTipoEscala_fl);
            return View(gatbl_EscalaCalificaciones);
        }

        // GET: gatbl_EscalaCalificaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones = db.gatbl_EscalaCalificaciones.Find(id);
            if (gatbl_EscalaCalificaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.lUniversidad_id = db.gatbl_Universidades.Select(g => new { lUniversidad_id = g.lUniversidad_id, sNombre_desc = g.sNombre_desc.Trim() }).OrderBy(g => g.sNombre_desc);
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_EscalaCalificaciones.lUniversidad_id);
            ViewBag.sTipoEscala_fl = new SelectList(db.TipoEscalaEvaluacions, "sTipoEscala_fl", "sDescripcion", gatbl_EscalaCalificaciones.sTipoEscala_fl);
            return View(gatbl_EscalaCalificaciones);
        }

        // POST: gatbl_EscalaCalificaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones)
        {
            if (ModelState.IsValid)
            {
                gatbl_EscalaCalificaciones.iEstado_fl = "1";
                gatbl_EscalaCalificaciones.iEliminado_fl = "1";
                gatbl_EscalaCalificaciones.sCreated_by = DateTime.Now.ToString();
                gatbl_EscalaCalificaciones.iConcurrencia_id = 1;

                db.Entry(gatbl_EscalaCalificaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lUniversidad_id = db.gatbl_Universidades.Select(g => new { lUniversidad_id = g.lUniversidad_id, sNombre_desc = g.sNombre_desc.Trim() }).OrderBy(g => g.sNombre_desc);
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_EscalaCalificaciones.lUniversidad_id);
            ViewBag.sTipoEscala_fl = new SelectList(db.TipoEscalaEvaluacions, "sTipoEscala_fl", "sDescripcion", gatbl_EscalaCalificaciones.sTipoEscala_fl);
            return View(gatbl_EscalaCalificaciones);
        }

        // GET: gatbl_EscalaCalificaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones = db.gatbl_EscalaCalificaciones.Find(id);
            if (gatbl_EscalaCalificaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_EscalaCalificaciones);
        }

        // POST: gatbl_EscalaCalificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones = db.gatbl_EscalaCalificaciones.Find(id);
            db.gatbl_EscalaCalificaciones.Remove(gatbl_EscalaCalificaciones);
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
