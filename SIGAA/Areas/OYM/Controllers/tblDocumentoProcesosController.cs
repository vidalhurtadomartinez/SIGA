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
    public class tblDocumentoProcesosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_procesoDocumentos_puedeVerIndice)]
         public ActionResult Index()
        {
            var tblDocumentoProceso = db.tblDocumentoProceso.Include(t => t.tblDocumento).Include(t => t.tblProceso).Include(t => t.tblCategoria);
            return View(tblDocumentoProceso.ToList());
        }

        public JsonResult ObtenerDocumentos(string text)
        {
            //ConvalidacionExternaViewModels convalidacionExterna = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            //if (string.IsNullOrEmpty(text) && convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id != null)
            //{
            //    return Json((from ag in db.agendas
            //                 select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json((from ag in db.agendas
            //                 select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            //}



            return Json((from doc in db.tblDocumento
                         where !(from dp in db.tblDocumentoProceso
                                 where dp.lDocumento_id == doc.Id && dp.lDocumento_version == doc.lVersion
                                 select dp.lDocumento_id).Contains(doc.Id)
                         select new { Id = doc.Id, Codigo = doc.sCodigo, Titulo = doc.Titulo, Version = doc.lVersion }).Where(g => g.Codigo.Contains(text) || g.Titulo.Contains(text)).OrderBy(g => g.Titulo), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoriaList(int id)
        {
            var proceso = db.tblProceso.Find(id);

            var categorias = from c in db.tblCategoria
                             where (from dp in db.tblProcesoDetalle
                                    where dp.lProceso_id == id
                                    select dp.lCategoria_id).Contains(c.lCategoria_id)
                             orderby c.lPrioridad ascending
                             select c;

            var categoriasgeneral = from c in db.tblCategoria
                                    where (from dp in db.tblProcesoDetalleCategoria
                                           where dp.lProceso_id == id
                                           select dp.lCategoria_id).Contains(c.lCategoria_id)
                                    orderby c.lPrioridad ascending
                                    select c;

            if (proceso.lTipoProceso_id == 1)
            {
                return Json(new SelectList(categoriasgeneral.ToArray(), "lCategoria_id", "sDescripcion"), JsonRequestBehavior.AllowGet);
            }                
            else
            {
                return Json(new SelectList(categorias.ToArray(), "lCategoria_id", "sDescripcion"), JsonRequestBehavior.AllowGet);
            }
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoDocumentos_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoProceso tblDocumentoProceso = db.tblDocumentoProceso.Find(id);
            if (tblDocumentoProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoDocumentos_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");
            ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion");
            ViewBag.ProcesoList = db.tblProceso;
            //return View(new tblDocumentoProceso { lEstadoProceso_id =1});

            return View();
        }

        // POST: tblDocumentoProcesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblDocumentoProceso tblDocumentoProceso)
        {
            if (ModelState.IsValid)
            {
                tblDocumentoProceso.dtFechaRegistro_dt = DateTime.Now;
                tblDocumentoProceso.lUsuario_id = FrontUser.Get().iUsuario_id;


                db.tblDocumentoProceso.Add(tblDocumentoProceso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoProceso.lDocumento_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblDocumentoProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblDocumentoProceso.lEstadoProceso_id);
            return View(tblDocumentoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoDocumentos_puedeCrearNuevo)]
        public ActionResult CreateProcess(int id)
        {
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");
            ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion");
            ViewBag.ProcesoList = db.tblProceso;
            
            var documento = db.tblDocumento.Find(id);
            var docprocess = db.tblDocumentoProceso.Where(t => t.lDocumento_id == documento.Id && t.lDocumento_version == documento.lVersion);

            if(docprocess.Count() > 0)
                ViewBag.Error = "El documento tiene un proceso pendiente, debe finalizar antes de iniciar un nuevo proceso.";


            return View(new tblDocumentoProceso(){lDocumento_id =id, lDocumento_version = documento.lVersion, tblDocumento = documento });
        }

        // POST: tblDocumentoProcesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProcess(tblDocumentoProceso tblDocumentoProceso)
        {
            try
            {
                var docprocess = new tblDocumentoProceso()
                {
                    lDocumento_id = tblDocumentoProceso.lDocumento_id,
                    lDocumento_version = tblDocumentoProceso.lDocumento_version,
                    dtFechaRegistro_dt = DateTime.Now,
                    dtFechaInicio_dt = tblDocumentoProceso.dtFechaInicio_dt,
                    sComentarios = tblDocumentoProceso.sComentarios,
                    lProceso_id = tblDocumentoProceso.lProceso_id,
                    lCategoria_id = tblDocumentoProceso.lCategoria_id,
                    lUsuario_id = FrontUser.Get().iUsuario_id
                };


                db.tblDocumentoProceso.Add(docprocess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {

            }
            
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoProceso.lDocumento_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblDocumentoProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblDocumentoProceso.lEstadoProceso_id);
            return View(tblDocumentoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoDocumentos_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoProceso tblDocumentoProceso = db.tblDocumentoProceso.Find(id);
            if (tblDocumentoProceso == null)
            {
                return HttpNotFound();
            }
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoProceso.lDocumento_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblDocumentoProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblDocumentoProceso.lEstadoProceso_id);
            return View(tblDocumentoProceso);
        }

        // POST: tblDocumentoProcesos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblDocumentoProceso tblDocumentoProceso)
        {
            if (ModelState.IsValid)
            {
                var docpro = db.tblDocumentoProceso.Find(tblDocumentoProceso.lDocumentoProceso_id);

                docpro.lDocumento_id = tblDocumentoProceso.lDocumento_id;
                docpro.lDocumento_version = tblDocumentoProceso.lDocumento_version;
                docpro.sComentarios = tblDocumentoProceso.sComentarios;
                docpro.dtFechaInicio_dt = tblDocumentoProceso.dtFechaInicio_dt;

                
                db.Entry(docpro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblDocumentoProceso.lDocumento_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblDocumentoProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblDocumentoProceso.lEstadoProceso_id);
            return View(tblDocumentoProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoDocumentos_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumentoProceso tblDocumentoProceso = db.tblDocumentoProceso.Find(id);
            if (tblDocumentoProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblDocumentoProceso);
        }

        // POST: tblDocumentoProcesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblDocumentoProceso tblDocumentoProceso = db.tblDocumentoProceso.Find(id);
            db.tblDocumentoProceso.Remove(tblDocumentoProceso);
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
