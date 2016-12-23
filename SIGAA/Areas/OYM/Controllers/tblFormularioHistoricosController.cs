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
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblFormularioHistoricosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblFormularioHistoricos
        public ActionResult Index()
        {
            var tblFormularioHistorico = db.tblFormularioHistorico.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblDocumento).Include(t => t.tblFormulario).Include(t => t.tblTipoProceso);
            return View(tblFormularioHistorico.ToList());
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.FormularioID = id;
            var formhistorico = db.tblFormularioHistorico.Where(a => a.lFormulario_id == id);

            return PartialView("_List", formhistorico.ToList());
        }

        public ActionResult PublicarFormulario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioHistorico tblFormularioHistorico = db.tblFormularioHistorico.Find(id);
            if (tblFormularioHistorico == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioHistorico);
        }

        // POST: tblDocumentoHistoricos/PublicarDocumento/5
        [HttpPost, ActionName("PublicarFormulario")]
        [ValidateAntiForgeryToken]
        public ActionResult PublicarConfirmed(int id)
        {
            tblFormularioHistorico tblFormularioHistorico = db.tblFormularioHistorico.Find(id);

            if (tblFormularioHistorico == null)
            {
                return HttpNotFound();
            }

            tblFormularioHistorico.dtFechaPublicacion_dt = DateTime.Now;

            db.Entry(tblFormularioHistorico).State = EntityState.Modified;


            var formpublicado = new tblFormularioPublicado()
            {
                dtFechaRegistro_dt = tblFormularioHistorico.dtFechaRegistro_dt,
                lFormularioHistorico_id = id,
                lFormulario_id = tblFormularioHistorico.lFormulario_id,
                lOrigenDocumento_id = tblFormularioHistorico.lOrigenDocumento_id,
                //lTipoDocumento_id = tblFormularioHistorico.lTipoDocumento_id,
                Titulo = tblFormularioHistorico.Titulo,
                lTipoProceso_id = tblFormularioHistorico.lTipoProceso_id,
                sCorrelativo = tblFormularioHistorico.sCorrelativo,
                sCodigo = tblFormularioHistorico.sCodigo,
                lVersion = tblFormularioHistorico.lVersion,
                dtValidoDesde_dt = tblFormularioHistorico.dtValidoDesde_dt,
                dtUltimaActualizacion_dt = tblFormularioHistorico.dtUltimaActualizacion_dt,
                NombreArchivo = tblFormularioHistorico.NombreArchivo,
                UbicacionArchivo = tblFormularioHistorico.UbicacionArchivo,
                lDirectorio_id = tblFormularioHistorico.lDirectorio_id,
                lControlCalidad_id = tblFormularioHistorico.lControlCalidad_id,
                lElaboradoPor_id = tblFormularioHistorico.lElaboradoPor_id,
                lRevisadoPor_id = tblFormularioHistorico.lRevisadoPor_id,
                lAprobadoPor_id = tblFormularioHistorico.lAprobadoPor_id,
                sComentarios = tblFormularioHistorico.sComentarios,
                lEstado_id = tblFormularioHistorico.lEstado_id,
                sISO = tblFormularioHistorico.sISO,
                lDocumento_id = tblFormularioHistorico.lDocumento_id,
                dtFechaCreacion_dt = tblFormularioHistorico.dtFechaCreacion_dt,
                lUsuario_id = FrontUser.Get().iUsuario_id
            };

            db.tblFormularioPublicado.Add(formpublicado);

            db.SaveChanges();

            return RedirectToAction("Index", "tblFormularios");
        }

        // GET: tblFormularioHistoricos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioHistorico tblFormularioHistorico = db.tblFormularioHistorico.Find(id);
            if (tblFormularioHistorico == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioHistorico);
        }

        // GET: tblFormularioHistoricos/Create
        public ActionResult Create()
        {
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1");
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1");
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1");
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion");
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion");
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1");
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre");
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo");
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo");
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla");
            return View();
        }

        // POST: tblFormularioHistoricos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lFormularioHistorico_id,dtFechaRegistro_dt,dtFechaPublicacion_dt,lFormulario_id,lOrigenDocumento_id,lDocumento_id,Titulo,lTipoProceso_id,sCorrelativo,sCodigo,lVersion,dtValidoDesde_dt,dtUltimaActualizacion_dt,NombreArchivo,UbicacionArchivo,lDirectorio_id,lControlCalidad_id,lElaboradoPor_id,lRevisadoPor_id,lAprobadoPor_id,sComentarios,lEstado_id,sISO")] tblFormularioHistorico tblFormularioHistorico)
        {
            if (ModelState.IsValid)
            {
                db.tblFormularioHistorico.Add(tblFormularioHistorico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormularioHistorico.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormularioHistorico.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblFormularioHistorico.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioHistorico.lDocumento_id);
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioHistorico.lFormulario_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblFormularioHistorico.lTipoProceso_id);
            return View(tblFormularioHistorico);
        }

        // GET: tblFormularioHistoricos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioHistorico tblFormularioHistorico = db.tblFormularioHistorico.Find(id);
            if (tblFormularioHistorico == null)
            {
                return HttpNotFound();
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormularioHistorico.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormularioHistorico.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblFormularioHistorico.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioHistorico.lDocumento_id);
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioHistorico.lFormulario_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblFormularioHistorico.lTipoProceso_id);
            return View(tblFormularioHistorico);
        }

        // POST: tblFormularioHistoricos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lFormularioHistorico_id,dtFechaRegistro_dt,dtFechaPublicacion_dt,lFormulario_id,lOrigenDocumento_id,lDocumento_id,Titulo,lTipoProceso_id,sCorrelativo,sCodigo,lVersion,dtValidoDesde_dt,dtUltimaActualizacion_dt,NombreArchivo,UbicacionArchivo,lDirectorio_id,lControlCalidad_id,lElaboradoPor_id,lRevisadoPor_id,lAprobadoPor_id,sComentarios,lEstado_id,sISO")] tblFormularioHistorico tblFormularioHistorico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblFormularioHistorico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormularioHistorico.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormularioHistorico.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioHistorico.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblFormularioHistorico.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioHistorico.lDocumento_id);
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioHistorico.lFormulario_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblFormularioHistorico.lTipoProceso_id);
            return View(tblFormularioHistorico);
        }

        // GET: tblFormularioHistoricos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioHistorico tblFormularioHistorico = db.tblFormularioHistorico.Find(id);
            if (tblFormularioHistorico == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioHistorico);
        }

        // POST: tblFormularioHistoricos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblFormularioHistorico tblFormularioHistorico = db.tblFormularioHistorico.Find(id);
            db.tblFormularioHistorico.Remove(tblFormularioHistorico);
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
