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
    public class gatbl_DPConvalidacionesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_DPConvalidaciones
        public ActionResult Index()
        {
            var gatbl_DPConvalidaciones = db.gatbl_DPConvalidaciones.Include(g => g.gatbl_PConvalidaciones).Include(g => g.TipoPresentacionDocumento);
            return View(gatbl_DPConvalidaciones.ToList());
        }

        // GET: gatbl_DPConvalidaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DPConvalidaciones gatbl_DPConvalidaciones = db.gatbl_DPConvalidaciones.Find(id);
            if (gatbl_DPConvalidaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_DPConvalidaciones);
        }

        // GET: gatbl_DPConvalidaciones/Create
        public ActionResult Create()
        {
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lPConvalidacion_id = new SelectList(db.gatbl_PConvalidaciones, "lPConvalidacion_id", "sGestion_desc");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sOrigen_fl");
            ViewBag.sTipoDocumento_fl = new SelectList(db.tipo_documentos, "doc_codigo", "doc_descripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            return View();
        }

        // POST: gatbl_DPConvalidaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lDPConvalicacion_id,lPConvalidacion_id,lUniversidad_id,lCarrera_id,sTipoDocumento_fl,sTipoPresentacion_fl,sCantidad_nro,sAprobados_nro,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_DPConvalidaciones gatbl_DPConvalidaciones)
        {
            if (ModelState.IsValid)
            {
                db.gatbl_DPConvalidaciones.Add(gatbl_DPConvalidaciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lPConvalidacion_id = new SelectList(db.gatbl_PConvalidaciones, "lPConvalidacion_id", "sGestion_desc", gatbl_DPConvalidaciones.lPConvalidacion_id);
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion", gatbl_DPConvalidaciones.lTipoDocumentoSolicitud_id);
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion", gatbl_DPConvalidaciones.sTipoPresentacion_fl);
            return View(gatbl_DPConvalidaciones);
        }

        // GET: gatbl_DPConvalidaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DPConvalidaciones gatbl_DPConvalidaciones = db.gatbl_DPConvalidaciones.Find(id);
            if (gatbl_DPConvalidaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.lPConvalidacion_id = new SelectList(db.gatbl_PConvalidaciones, "lPConvalidacion_id", "sGestion_desc", gatbl_DPConvalidaciones.lPConvalidacion_id);
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion", gatbl_DPConvalidaciones.lTipoDocumentoSolicitud_id);
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion", gatbl_DPConvalidaciones.sTipoPresentacion_fl);
            return View(gatbl_DPConvalidaciones);
        }

        // POST: gatbl_DPConvalidaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lDPConvalicacion_id,lPConvalidacion_id,lUniversidad_id,lCarrera_id,sTipoDocumento_fl,sTipoPresentacion_fl,sCantidad_nro,sAprobados_nro,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_DPConvalidaciones gatbl_DPConvalidaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatbl_DPConvalidaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lPConvalidacion_id = new SelectList(db.gatbl_PConvalidaciones, "lPConvalidacion_id", "sGestion_desc", gatbl_DPConvalidaciones.lPConvalidacion_id);
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion", gatbl_DPConvalidaciones.lTipoDocumentoSolicitud_id);
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion", gatbl_DPConvalidaciones.sTipoPresentacion_fl);
            return View(gatbl_DPConvalidaciones);
        }

        // GET: gatbl_DPConvalidaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DPConvalidaciones gatbl_DPConvalidaciones = db.gatbl_DPConvalidaciones.Find(id);
            if (gatbl_DPConvalidaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_DPConvalidaciones);
        }

        // POST: gatbl_DPConvalidaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_DPConvalidaciones gatbl_DPConvalidaciones = db.gatbl_DPConvalidaciones.Find(id);
            db.gatbl_DPConvalidaciones.Remove(gatbl_DPConvalidaciones);
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
