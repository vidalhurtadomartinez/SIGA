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
    public class tblDocumentoHistoricosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblDocumentoHistoricos
        public ActionResult Index()
        {
            var tblDocumentoHistorico = db.tblDocumentoHistorico.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblDocumento).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso);
            return View(tblDocumentoHistorico.ToList());
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.DocumentID = id;
            var dochistorico = db.tblDocumentoHistorico.Where(a => a.lDocumento_id == id);

            return PartialView("_List", dochistorico.ToList());
        }

        public ActionResult PublicarDocumento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoHistorico tblDocumentoHistorico = db.tblDocumentoHistorico.Find(id);
            if (tblDocumentoHistorico == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoHistorico);
        }

        // POST: tblDocumentoHistoricos/PublicarDocumento/5
        [HttpPost, ActionName("PublicarDocumento")]
        [ValidateAntiForgeryToken]
        public ActionResult PublicarConfirmed(int id)
        {
            tblDocumentoHistorico tblDocumentoHistorico = db.tblDocumentoHistorico.Find(id);

            if (tblDocumentoHistorico == null)
            {
                return HttpNotFound();
            }

            tblDocumentoHistorico.dtFechaPublicacion_dt = DateTime.Now;

            db.Entry(tblDocumentoHistorico).State = EntityState.Modified;


            var docpublicado = new tblDocumentoPublicado()
            {
                dtFechaRegistro_dt = tblDocumentoHistorico.dtFechaRegistro_dt,
                lDocumentoHistorico_id =  id,
                lOrigenDocumento_id = tblDocumentoHistorico.lOrigenDocumento_id,
                lTipoDocumento_id = tblDocumentoHistorico.lTipoDocumento_id,
                Titulo = tblDocumentoHistorico.Titulo,
                lTipoProceso_id = tblDocumentoHistorico.lTipoProceso_id,
                sCorrelativo = tblDocumentoHistorico.sCorrelativo,
                sCodigo = tblDocumentoHistorico.sCodigo,
                lVersion = tblDocumentoHistorico.lVersion,
                dtValidoDesde_dt = tblDocumentoHistorico.dtValidoDesde_dt,
                dtUltimaActualizacion_dt = tblDocumentoHistorico.dtUltimaActualizacion_dt,
                NombreArchivo = tblDocumentoHistorico.NombreArchivo,
                UbicacionArchivo = tblDocumentoHistorico.UbicacionArchivo,
                lPlantilla_id = tblDocumentoHistorico.lPlantilla_id,
                lDirectorio_id = tblDocumentoHistorico.lDirectorio_id,
                lControlCalidad_id = tblDocumentoHistorico.lControlCalidad_id,
                lElaboradoPor_id = tblDocumentoHistorico.lElaboradoPor_id,
                lRevisadoPor_id = tblDocumentoHistorico.lRevisadoPor_id,
                lAprobadoPor_id = tblDocumentoHistorico.lAprobadoPor_id,
                sComentarios = tblDocumentoHistorico.sComentarios,
                lEstado_id = tblDocumentoHistorico.lEstado_id,
                sISO = tblDocumentoHistorico.sISO,
                lDocumento_id = tblDocumentoHistorico.lDocumento_id,
                dtFechaCreacion_dt = tblDocumentoHistorico.dtFechaCreacion_dt,
                lUsuario_id = FrontUser.Get().iUsuario_id
            };

            db.tblDocumentoPublicado.Add(docpublicado);

            db.SaveChanges();        

            return RedirectToAction("Index","tblDocumentos");
        }

        // GET: tblDocumentoHistoricos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoHistorico tblDocumentoHistorico = db.tblDocumentoHistorico.Find(id);
            if (tblDocumentoHistorico == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoHistorico);
        }

        // GET: tblDocumentoHistoricos/Create
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
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre");
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla");
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla");
            return View();
        }

        // POST: tblDocumentoHistoricos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lDocumentoHistorico_id,dtFechaRegistro_dt,lDocumento_id,lOrigenDocumento_id,dtFechaCreacion_dt,lTipoDocumento_id,Titulo,lTipoProceso_id,sCorrelativo,sCodigo,lVersion,dtValidoDesde_dt,dtUltimaActualizacion_dt,NombreArchivo,UbicacionArchivo,lPlantilla_id,lDirectorio_id,lControlCalidad_id,lElaboradoPor_id,lRevisadoPor_id,lAprobadoPor_id,sComentarios,lEstado_id,sISO")] tblDocumentoHistorico tblDocumentoHistorico)
        {
            if (ModelState.IsValid)
            {
                db.tblDocumentoHistorico.Add(tblDocumentoHistorico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumentoHistorico.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumentoHistorico.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDocumentoHistorico.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoHistorico.lDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblDocumentoHistorico.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla", tblDocumentoHistorico.lTipoDocumento_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblDocumentoHistorico.lTipoProceso_id);
            return View(tblDocumentoHistorico);
        }

        // GET: tblDocumentoHistoricos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoHistorico tblDocumentoHistorico = db.tblDocumentoHistorico.Find(id);
            if (tblDocumentoHistorico == null)
            {
                return HttpNotFound();
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumentoHistorico.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumentoHistorico.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDocumentoHistorico.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoHistorico.lDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblDocumentoHistorico.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla", tblDocumentoHistorico.lTipoDocumento_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblDocumentoHistorico.lTipoProceso_id);
            return View(tblDocumentoHistorico);
        }

        // POST: tblDocumentoHistoricos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lDocumentoHistorico_id,dtFechaRegistro_dt,lDocumento_id,lOrigenDocumento_id,dtFechaCreacion_dt,lTipoDocumento_id,Titulo,lTipoProceso_id,sCorrelativo,sCodigo,lVersion,dtValidoDesde_dt,dtUltimaActualizacion_dt,NombreArchivo,UbicacionArchivo,lPlantilla_id,lDirectorio_id,lControlCalidad_id,lElaboradoPor_id,lRevisadoPor_id,lAprobadoPor_id,sComentarios,lEstado_id,sISO")] tblDocumentoHistorico tblDocumentoHistorico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblDocumentoHistorico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumentoHistorico.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumentoHistorico.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoHistorico.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDocumentoHistorico.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoHistorico.lDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblDocumentoHistorico.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla", tblDocumentoHistorico.lTipoDocumento_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblDocumentoHistorico.lTipoProceso_id);
            return View(tblDocumentoHistorico);
        }

        // GET: tblDocumentoHistoricos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoHistorico tblDocumentoHistorico = db.tblDocumentoHistorico.Find(id);
            if (tblDocumentoHistorico == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoHistorico);
        }

        // POST: tblDocumentoHistoricos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblDocumentoHistorico tblDocumentoHistorico = db.tblDocumentoHistorico.Find(id);
            db.tblDocumentoHistorico.Remove(tblDocumentoHistorico);
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
