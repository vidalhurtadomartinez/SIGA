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
    public class tblFormularioPublicadosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_formulariosPublicados_puedeVerIndice)]
        public ActionResult Index()
        {
            var tblFormularioPublicado = db.tblFormularioPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblDocumento).Include(t => t.tblFormulario).Include(t => t.tblFormularioHistorico).Include(t => t.tblTipoProceso);
            return View(tblFormularioPublicado.ToList());
        }

        public ActionResult IndexByDirectory(int id)
        {
            var directorio = db.tblDirectorio.Find(id);
            return View(directorio);
        }

        public ActionResult FilterByDirectory()
        {
            var tblFormularioPublicado = db.tblFormularioPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblDocumento).Include(t => t.tblFormulario).Include(t => t.tblFormularioHistorico).Include(t => t.tblTipoProceso);
            return View(tblFormularioPublicado.ToList());
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

        [Permiso(Permiso = RolesPermisos.OYM_formulariosPublicados_puedeVerIndice)]
        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var directorio = db.tblDirectorio.Find(id);

            var doc = from c in db.tblFormularioPublicado.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDirectorio).Include(t => t.tblDocumento).Include(t => t.tblFormulario).Include(t => t.tblFormularioHistorico).Include(t => t.tblTipoProceso)
                      where db.tblDirectorio.Where(m => m.sCodigo.StartsWith(directorio.sCodigo)).Any(m => m.lDirectorio_id == c.lDirectorio_id)
                      &&
                      (from d in db.tblFormularioPublicado.GroupBy(m => m.lFormulario_id).Select(g => new
                      {
                          lFormulario_id = g.Key,
                          uVersion = g.Max(row => row.lVersion)
                      }
                          ).Where(m => m.uVersion == c.lVersion)
                             select d.lFormulario_id).Contains(c.lFormulario_id)
                      select c;

            return Json(doc.ToDataSourceResult(request, c => new
            {
                c.lFormularioPublicado_id,
                c.lFormulario_id,
                c.lDocumento_id,
                c.dtFechaCreacion_dt,
                c.Titulo,
                c.NombreArchivo,
                c.lOrigenDocumento_id,
                c.lTipoProceso_id,
                c.sCorrelativo,
                c.sCodigo,
                c.lVersion,
                c.dtValidoDesde_dt,
                c.dtUltimaActualizacion_dt,
                c.UbicacionArchivo,
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
                tblTipoProceso = new
                {
                    c.lTipoProceso_id,
                    c.tblTipoProceso.sDescripcion
                },
                EstadoDocumento = new
                {
                    c.lEstado_id,
                    c.EstadoDocumento.sDescripcion
                }
            }));
        }

        [Permiso(Permiso = RolesPermisos.OYM_formulariosPublicados_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioPublicado tblFormularioPublicado = db.tblFormularioPublicado.Find(id);
            if (tblFormularioPublicado == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioPublicado);
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
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo");
            ViewBag.lFormularioHistorico_id = new SelectList(db.tblFormularioHistorico, "lFormularioHistorico_id", "Titulo");
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla");
            return View();
        }

        // POST: tblFormularioPublicados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblFormularioPublicado tblFormularioPublicado)
        {
            if (ModelState.IsValid)
            {
                tblFormularioPublicado.lUsuario_id = FrontUser.Get().iUsuario_id;

                db.tblFormularioPublicado.Add(tblFormularioPublicado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormularioPublicado.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormularioPublicado.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblFormularioPublicado.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioPublicado.lDocumento_id);
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioPublicado.lFormulario_id);
            ViewBag.lFormularioHistorico_id = new SelectList(db.tblFormularioHistorico, "lFormularioHistorico_id", "Titulo", tblFormularioPublicado.lFormularioHistorico_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblFormularioPublicado.lTipoProceso_id);
            return View(tblFormularioPublicado);
        }

        // GET: tblFormularioPublicados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioPublicado tblFormularioPublicado = db.tblFormularioPublicado.Find(id);
            if (tblFormularioPublicado == null)
            {
                return HttpNotFound();
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormularioPublicado.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormularioPublicado.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblFormularioPublicado.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioPublicado.lDocumento_id);
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioPublicado.lFormulario_id);
            ViewBag.lFormularioHistorico_id = new SelectList(db.tblFormularioHistorico, "lFormularioHistorico_id", "Titulo", tblFormularioPublicado.lFormularioHistorico_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblFormularioPublicado.lTipoProceso_id);
            return View(tblFormularioPublicado);
        }

        // POST: tblFormularioPublicados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lFormularioPublicado_id,dtFechaRegistro_dt,lFormularioHistorico_id,lFormulario_id,dtFechaCreacion_dt,lOrigenDocumento_id,lDocumento_id,Titulo,lTipoProceso_id,sCorrelativo,sCodigo,lVersion,dtValidoDesde_dt,dtUltimaActualizacion_dt,NombreArchivo,UbicacionArchivo,lDirectorio_id,lControlCalidad_id,lElaboradoPor_id,lRevisadoPor_id,lAprobadoPor_id,sComentarios,lEstado_id,sISO")] tblFormularioPublicado tblFormularioPublicado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblFormularioPublicado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lAprobadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lAprobadoPor_id);
            ViewBag.lControlCalidad_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lControlCalidad_id);
            ViewBag.lElaboradoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lElaboradoPor_id);
            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormularioPublicado.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormularioPublicado.lOrigenDocumento_id);
            ViewBag.lRevisadoPor_id = new SelectList(db.vVistaPersonal, "idcont", "CodCargoNivel1", tblFormularioPublicado.lRevisadoPor_id);
            ViewBag.lDirectorio_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblFormularioPublicado.lDirectorio_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioPublicado.lDocumento_id);
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioPublicado.lFormulario_id);
            ViewBag.lFormularioHistorico_id = new SelectList(db.tblFormularioHistorico, "lFormularioHistorico_id", "Titulo", tblFormularioPublicado.lFormularioHistorico_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sSigla", tblFormularioPublicado.lTipoProceso_id);
            return View(tblFormularioPublicado);
        }

        // GET: tblFormularioPublicados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioPublicado tblFormularioPublicado = db.tblFormularioPublicado.Find(id);
            if (tblFormularioPublicado == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioPublicado);
        }

        // POST: tblFormularioPublicados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblFormularioPublicado tblFormularioPublicado = db.tblFormularioPublicado.Find(id);
            db.tblFormularioPublicado.Remove(tblFormularioPublicado);
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
