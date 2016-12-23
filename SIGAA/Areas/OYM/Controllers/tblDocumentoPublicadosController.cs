using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblDocumentoPublicadosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_documentosPublicados_puedeVerIndice)]
        public ActionResult Index()
        {
            var tblDocumentoPublicado = db.tblDocumentoPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso);
            return View(tblDocumentoPublicado.ToList());
        }

        public ActionResult IndexByDirectory(int id)
        {
            //var tblDocumentoPublicado = db.tblDocumentoPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio);
            //return View(tblDocumentoPublicado.ToList());

            var directorio = db.tblDirectorio.Find(id);
            return View(directorio);
        }

        public ActionResult FilterByDirectory()
        {
            var tblDocumentoPublicado = db.tblDocumentoPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso);
            return View(tblDocumentoPublicado.ToList());
        }

        public JsonResult GetDirectory([DataSourceRequest] DataSourceRequest request)
        {            
            var post = from p in db.tblDirectorio.Include(g => g.DirectorioPadre)
                       orderby p.sNombre ascending
                       where p.bActivo == true
                       select p;
            return Json(post.ToDataSourceResult(request, p => new
            {
                p.lDirectorio_id,
                p.sNombre,
                p.sDescripcion,
                p.lDirectorioPadre_id,
                p.bActivo
            }));
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentosPublicados_puedeVerIndice)]
        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            //var doc = from c in db.tblDocumentoPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio)
            //          select c;

            var directorio = db.tblDirectorio.Find(id);

            var doc = from c in db.tblDocumentoPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio)
                      where db.tblDirectorio.Where(m=>m.sCodigo.StartsWith(directorio.sCodigo)).Any(m => m.lDirectorio_id == c.lDirectorio_id)
                      &&
                      (from d in db.tblDocumentoPublicado.GroupBy(m => m.lDocumento_id).Select(g => new
                      {
                          lDocumento_id = g.Key,
                          uVersion = g.Max(row => row.lVersion)
                      }
                          ).Where(m => m.uVersion == c.lVersion) select d.lDocumento_id).Contains(c.lDocumento_id)                             
                      select c;            
            
            return Json(doc.ToDataSourceResult(request, c => new
            {
                c.lDocumentoPlublicado_id,
                c.lDocumento_id,
                c.dtFechaCreacion_dt,
                c.Titulo,
                c.NombreArchivo,
                c.lOrigenDocumento_id,
                c.lTipoDocumento_id,
                c.lTipoProceso_id,
                c.sCorrelativo,
                c.sCodigo,
                c.lVersion,
                c.dtValidoDesde_dt,
                c.dtUltimaActualizacion_dt,
                c.UbicacionArchivo,
                c.lPlantilla_id,
                c.sComentarios,
                c.lEstado_id,
                c.sISO,
                c.lControlCalidad_id,
                c.lElaboradoPor_id,
                c.lRevisadoPor_id,
                c.lAprobadoPor_id,
                c.lDirectorio_id,
                OrigenDocumento = new
                {
                    c.lOrigenDocumento_id,
                    c.OrigenDocumento.sDescripcion
                },
                tblTipoDocumento = new
                {
                    c.lTipoDocumento_id,
                    c.tblTipoDocumento.sTipoDocumento
                },
                tblTipoProceso = new
                {
                    c.lTipoProceso_id,
                    c.tblTipoProceso.sDescripcion
                },
                tblPlantilla = new
                {
                    c.lPlantilla_id,
                    c.tblPlantilla.Nombre,
                    c.tblPlantilla.Descripcion
                },
                tblDirectorio = new
                {
                    c.lDirectorio_id,
                    c.tblDirectorio.sNombre,
                    c.tblDirectorio.sDescripcion
                },
                EstadoDocumento = new
                {
                    c.lEstado_id,
                    c.EstadoDocumento.sDescripcion
                }
            }));
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentosPublicados_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoPublicado tblDocumentoPublicado = db.tblDocumentoPublicado.Find(id);
            if (tblDocumentoPublicado == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoPublicado);
        }

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

        // POST: tblDocumentoPublicados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblDocumentoPublicado tblDocumentoPublicado)
        {
            if (ModelState.IsValid)
            {
                tblDocumentoPublicado.lUsuario_id = FrontUser.Get().iUsuario_id;

                db.tblDocumentoPublicado.Add(tblDocumentoPublicado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumentoPublicado.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumentoPublicado.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDocumentoPublicado.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoPublicado.lDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblDocumentoPublicado.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla", tblDocumentoPublicado.lTipoDocumento_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblDocumentoPublicado.lTipoProceso_id);
            return View(tblDocumentoPublicado);
        }

        // GET: tblDocumentoPublicados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoPublicado tblDocumentoPublicado = db.tblDocumentoPublicado.Find(id);
            if (tblDocumentoPublicado == null)
            {
                return HttpNotFound();
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumentoPublicado.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumentoPublicado.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDocumentoPublicado.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoPublicado.lDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblDocumentoPublicado.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla", tblDocumentoPublicado.lTipoDocumento_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblDocumentoPublicado.lTipoProceso_id);
            return View(tblDocumentoPublicado);
        }

        // POST: tblDocumentoPublicados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lDocumentoPlublicado_id,dtFechaRegistro_dt,lDocumento_id,lOrigenDocumento_id,dtFechaCreacion_dt,lTipoDocumento_id,Titulo,lTipoProceso_id,sCorrelativo,sCodigo,lVersion,dtValidoDesde_dt,dtUltimaActualizacion_dt,NombreArchivo,UbicacionArchivo,lPlantilla_id,lDirectorio_id,lControlCalidad_id,lElaboradoPor_id,lRevisadoPor_id,lAprobadoPor_id,sComentarios,lEstado_id,sISO")] tblDocumentoPublicado tblDocumentoPublicado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblDocumentoPublicado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumentoPublicado.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumentoPublicado.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblDocumentoPublicado.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDocumentoPublicado.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoPublicado.lDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblDocumentoPublicado.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sSigla", tblDocumentoPublicado.lTipoDocumento_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblDocumentoPublicado.lTipoProceso_id);
            return View(tblDocumentoPublicado);
        }

        // GET: tblDocumentoPublicados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoPublicado tblDocumentoPublicado = db.tblDocumentoPublicado.Find(id);
            if (tblDocumentoPublicado == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoPublicado);
        }

        // POST: tblDocumentoPublicados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblDocumentoPublicado tblDocumentoPublicado = db.tblDocumentoPublicado.Find(id);
            db.tblDocumentoPublicado.Remove(tblDocumentoPublicado);
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
