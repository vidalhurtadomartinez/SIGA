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
    public class gatbl_DProgramasAnaliticosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_DProgramasAnaliticos
        public ActionResult Index()
        {
            var gatbl_DProgramasAnaliticos = db.gatbl_DProgramasAnaliticos.Include(g => g.gatbl_ProgramasAnaliticos);
            return View(gatbl_DProgramasAnaliticos.ToList());
        }

        // GET: gatbl_DProgramasAnaliticos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos = db.gatbl_DProgramasAnaliticos.Find(id);
            if (gatbl_DProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_DProgramasAnaliticos);
        }

        // GET: gatbl_DProgramasAnaliticos/Create
        public ActionResult Create()
        {
            ViewBag.lProgramaAnalitico_id = new SelectList(db.gatbl_ProgramasAnaliticos, "lProgramaAnalitico_id", "sGestion_desc");
            return View();
        }

        // POST: gatbl_DProgramasAnaliticos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lDProgramaAnalitico_id,lProgramaAnalitico_id,sUnidad_nro,sUnidad_desc,sContenido_gral,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos)
        {
            if (ModelState.IsValid)
            {
                db.gatbl_DProgramasAnaliticos.Add(gatbl_DProgramasAnaliticos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lProgramaAnalitico_id = new SelectList(db.gatbl_ProgramasAnaliticos, "lProgramaAnalitico_id", "sGestion_desc", gatbl_DProgramasAnaliticos.lProgramaAnalitico_id);
            return View(gatbl_DProgramasAnaliticos);
        }

        // GET: gatbl_DProgramasAnaliticos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos = db.gatbl_DProgramasAnaliticos.Find(id);
            if (gatbl_DProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }
            ViewBag.lProgramaAnalitico_id = new SelectList(db.gatbl_ProgramasAnaliticos, "lProgramaAnalitico_id", "sGestion_desc", gatbl_DProgramasAnaliticos.lProgramaAnalitico_id);
            return View(gatbl_DProgramasAnaliticos);
        }

        // POST: gatbl_DProgramasAnaliticos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lDProgramaAnalitico_id,lProgramaAnalitico_id,sUnidad_nro,sUnidad_desc,sContenido_gral,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatbl_DProgramasAnaliticos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lProgramaAnalitico_id = new SelectList(db.gatbl_ProgramasAnaliticos, "lProgramaAnalitico_id", "sGestion_desc", gatbl_DProgramasAnaliticos.lProgramaAnalitico_id);
            return View(gatbl_DProgramasAnaliticos);
        }

        // GET: gatbl_DProgramasAnaliticos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos = db.gatbl_DProgramasAnaliticos.Find(id);
            if (gatbl_DProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_DProgramasAnaliticos);
        }

        // POST: gatbl_DProgramasAnaliticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos = db.gatbl_DProgramasAnaliticos.Find(id);
            db.gatbl_DProgramasAnaliticos.Remove(gatbl_DProgramasAnaliticos);
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
