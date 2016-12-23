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
    public class tblFormularioProcesosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_procesoFormularios_puedeVerIndice)]
        public ActionResult Index()
        {
            var tblFormularioProceso = db.tblFormularioProceso.Include(t => t.tblFormulario).Include(t => t.tblProceso).Include(t => t.tblCategoria);
            return View(tblFormularioProceso.ToList());
        }

        public JsonResult ObtenerFormularios(string text)
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



            return Json((from doc in db.tblFormulario
                         where !(from dp in db.tblFormularioProceso
                                 select dp.lFormulario_id).Contains(doc.Id)
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

        [Permiso(Permiso = RolesPermisos.OYM_procesoFormularios_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioProceso tblFormularioProceso = db.tblFormularioProceso.Find(id);
            if (tblFormularioProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoFormularios_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");
            ViewBag.ProcesoList = db.tblProceso;
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion");
            return View();
        }

        // POST: tblFormularioProcesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblFormularioProceso tblFormularioProceso)
        {
            if (ModelState.IsValid)
            {
                tblFormularioProceso.dtFechaRegistro_dt = DateTime.Now;
                tblFormularioProceso.lUsuario_id = FrontUser.Get().iUsuario_id;

                db.tblFormularioProceso.Add(tblFormularioProceso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioProceso.lFormulario_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblFormularioProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblFormularioProceso.lEstadoProceso_id);
            return View(tblFormularioProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoFormularios_puedeCrearNuevo)]
        public ActionResult CreateProcess(int id)
        {
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "Titulo");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");
            ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion");
            ViewBag.ProcesoList = db.tblProceso;

            var formulario = db.tblFormulario.Find(id);
            var formprocess = db.tblDocumentoProceso.Where(t => t.lDocumento_id == formulario.Id && t.lDocumento_version == formulario.lVersion);

            if (formprocess.Count() > 0)
                ViewBag.Error = "El formulario tiene un proceso pendiente, debe finalizar antes de iniciar un nuevo proceso.";


            return View(new tblFormularioProceso() { lFormulario_id = id, lFormulario_version = formulario.lVersion, tblFormulario = formulario });
        }

        // POST: tblDocumentoProcesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProcess(tblFormularioProceso tblFormularioProceso)
        {
            try
            {
                var formprocess = new tblFormularioProceso()
                {
                    lFormulario_id = tblFormularioProceso.lFormulario_id,
                    lFormulario_version = tblFormularioProceso.lFormulario_version,
                    dtFechaRegistro_dt = DateTime.Now,
                    dtFechaInicio_dt = tblFormularioProceso.dtFechaInicio_dt,
                    sComentarios = tblFormularioProceso.sComentarios,
                    lProceso_id = tblFormularioProceso.lProceso_id,
                    lCategoria_id = tblFormularioProceso.lCategoria_id
                };


                db.tblFormularioProceso.Add(formprocess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            
            ViewBag.lFormulario_id = new SelectList(db.tblDocumento, "Id", "Titulo", tblFormularioProceso.lFormulario_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblFormularioProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblDocumentoProceso.lEstadoProceso_id);
            return View(tblFormularioProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoFormularios_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioProceso tblFormularioProceso = db.tblFormularioProceso.Find(id);
            if (tblFormularioProceso == null)
            {
                return HttpNotFound();
            }
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioProceso.lFormulario_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblFormularioProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblFormularioProceso.lEstadoProceso_id);
            return View(tblFormularioProceso);
        }

        // POST: tblFormularioProcesos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblFormularioProceso tblFormularioProceso)
        {
            if (ModelState.IsValid)
            {
                var formpro = db.tblFormularioProceso.Find(tblFormularioProceso.lFormularioProceso_id);

                formpro.lFormulario_id = tblFormularioProceso.lFormulario_id;
                formpro.lFormulario_version = tblFormularioProceso.lFormulario_version;
                formpro.sComentarios = tblFormularioProceso.sComentarios;
                formpro.dtFechaInicio_dt = tblFormularioProceso.dtFechaInicio_dt;

                db.Entry(formpro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lFormulario_id = new SelectList(db.tblFormulario, "Id", "Titulo", tblFormularioProceso.lFormulario_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblFormularioProceso.lProceso_id);
            //ViewBag.lEstadoProceso_id = new SelectList(db.EstadoProceso, "lEstadoProceso_id", "sDescripcion", tblFormularioProceso.lEstadoProceso_id);
            return View(tblFormularioProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesoFormularios_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormularioProceso tblFormularioProceso = db.tblFormularioProceso.Find(id);
            if (tblFormularioProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblFormularioProceso);
        }

        // POST: tblFormularioProcesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblFormularioProceso tblFormularioProceso = db.tblFormularioProceso.Find(id);
            db.tblFormularioProceso.Remove(tblFormularioProceso);
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
