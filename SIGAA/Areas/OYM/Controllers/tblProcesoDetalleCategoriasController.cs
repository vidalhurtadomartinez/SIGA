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

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblProcesoDetalleCategoriasController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblProcesoDetalleCategoriaCategorias
        public ActionResult Index(int id)
        {
            ViewBag.ProcessID = id;
            //var tblProcesoDetalleCategoria = db.tblProcesoDetalleCategoria.Include(t => t.agenda).Include(t => t.RolParticipante).Include(t => t.tblCategoria).Include(t => t.tblProceso);

            var tblProcesoDetalleCategoria = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalleCategorias;

            return PartialView("_Index", tblProcesoDetalleCategoria.ToList());
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.ProcessID = id;
            var tblProcesoDetalleCategoria = db.tblProcesoDetalleCategoria.Where(a => a.lProceso_id == id);

            return PartialView("_List", tblProcesoDetalleCategoria.ToList());
        }

        public JsonResult ObtenerParticipantes(string text)
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



            return Json((from ag in db.agenda
                         select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerParticipantesPersonal(string text)
        {
            return Json((from p in db.vVistaPersonal
                         select new { Id = p.idcont, Nombre = p.Nombre }).Where(g => g.Nombre.Contains(text)).OrderBy(g => g.Nombre), JsonRequestBehavior.AllowGet);
        }

        // GET: tblProcesoDetalleCategorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProcesoDetalleCategoria tblProcesoDetalleCategoria = db.tblProcesoDetalleCategoria.Find(id);
            if (tblProcesoDetalleCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tblProcesoDetalleCategoria);
        }

        // GET: tblProcesoDetalleCategorias/Create
        public ActionResult Create(int ProcessID)
        {
            tblProcesoDetalleCategoria tblProcesoDetalleCategoria = new tblProcesoDetalleCategoria();
            tblProcesoDetalleCategoria.lProceso_id = ProcessID;


            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo");
            //ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre");
            //ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion");
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");

            return PartialView("_Create", tblProcesoDetalleCategoria);
        }

        // POST: tblProcesoDetalleCategorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblProcesoDetalleCategoria tblProcesoDetalleCategoria)
        {
            if (ModelState.IsValid)
            {
                //db.tblProcesoDetalleCategoria.Add(tblProcesoDetalleCategoria);
                //db.SaveChanges();

                //var agenda = db.agenda.Find(tblProcesoDetalleCategoria.agd_codigo);
                var categoria = db.tblCategoria.Find(tblProcesoDetalleCategoria.lCategoria_id);
                
                ViewModels.Proceso proceso = (ViewModels.Proceso)Session["vProceso"];

                int DetalleId = proceso.tblProcesoDetalleCategorias.Count() + 1;
                tblProcesoDetalleCategoria.lProcesoDetalleCategoria_id = -DetalleId;
                 tblProcesoDetalleCategoria.tblCategoria = categoria;
                
                proceso.tblProcesoDetalleCategorias.Add(tblProcesoDetalleCategoria);

                string url = Url.Action("Index", "tblProcesoDetalleCategorias", new { id = tblProcesoDetalleCategoria.lProceso_id });
                return Json(new { success = true, url = url });

            }

            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo", tblProcesoDetalleCategoria.agd_codigo);
            //ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre", tblProcesoDetalleCategoria.lParticipante_id);
            //ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion", tblProcesoDetalleCategoria.lRolParticipante_id);
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion", tblProcesoDetalleCategoria.lCategoria_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblProcesoDetalleCategoria.lProceso_id);

            return PartialView("_Create", tblProcesoDetalleCategoria);
        }

        // GET: tblProcesoDetalleCategorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblProcesoDetalleCategoria tblProcesoDetalleCategoria = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalleCategorias.FirstOrDefault(t => t.lProcesoDetalleCategoria_id == id);
            if (tblProcesoDetalleCategoria == null)
            {
                return HttpNotFound();
            }



            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo", tblProcesoDetalleCategoria.agd_codigo);
            //ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre", tblProcesoDetalleCategoria.lParticipante_id);
            //ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion", tblProcesoDetalleCategoria.lRolParticipante_id);
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion", tblProcesoDetalleCategoria.lCategoria_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblProcesoDetalleCategoria.lProceso_id);

            return PartialView("_Edit", tblProcesoDetalleCategoria);
        }

        // POST: tblProcesoDetalleCategorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblProcesoDetalleCategoria tblProcesoDetalleCategoria)
        {
            if (ModelState.IsValid)
            {
                tblProcesoDetalleCategoria procesoactual = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalleCategorias.FirstOrDefault(t => t.lProcesoDetalleCategoria_id == tblProcesoDetalleCategoria.lProcesoDetalleCategoria_id);

                var categoria = db.tblCategoria.Find(tblProcesoDetalleCategoria.lCategoria_id);
                
                if (procesoactual != null)
                {
                    procesoactual.lProcesoDetalleCategoria_id = tblProcesoDetalleCategoria.lProcesoDetalleCategoria_id;
                    procesoactual.lCategoria_id = tblProcesoDetalleCategoria.lCategoria_id;
                    procesoactual.lProceso_id = tblProcesoDetalleCategoria.lProceso_id;
                    procesoactual.lRevision1 = tblProcesoDetalleCategoria.lRevision1;
                    procesoactual.lRevision2 = tblProcesoDetalleCategoria.lRevision2;
                    procesoactual.lRevision3 = tblProcesoDetalleCategoria.lRevision3;

                    procesoactual.tblCategoria = categoria;                    
                }

                //db.Entry(tblProcesoDetalleCategoria).State = EntityState.Modified;
                //db.SaveChanges();

                string url = Url.Action("Index", "tblProcesoDetalleCategorias", new { id = tblProcesoDetalleCategoria.lProceso_id });
                return Json(new { success = true, url = url });
            }
            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo", tblProcesoDetalleCategoria.agd_codigo);
            //ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre", tblProcesoDetalleCategoria.lParticipante_id);
            //ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion", tblProcesoDetalleCategoria.lRolParticipante_id);
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion", tblProcesoDetalleCategoria.lCategoria_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblProcesoDetalleCategoria.lProceso_id);

            return PartialView("_Edit", tblProcesoDetalleCategoria);
        }

        // GET: tblProcesoDetalleCategorias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblProcesoDetalleCategoria tblProcesoDetalleCategoria = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalleCategorias.FirstOrDefault(t => t.lProcesoDetalleCategoria_id == id);
            if (tblProcesoDetalleCategoria == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", tblProcesoDetalleCategoria);
        }

        // POST: tblProcesoDetalleCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //tblProcesoDetalleCategoria tblProcesoDetalleCategoria = db.tblProcesoDetalleCategoria.Find(id);
            //db.tblProcesoDetalleCategoria.Remove(tblProcesoDetalleCategoria);
            //db.SaveChanges();
            //return RedirectToAction("Index");

            var proceso = (ViewModels.Proceso)Session["vProceso"];
            int ProcessID = 0;
            if (proceso != null)
            {
                tblProcesoDetalleCategoria tblProcesoDetalleCategoria = proceso.tblProcesoDetalleCategorias.FirstOrDefault(t => t.lProcesoDetalleCategoria_id == id);

                if (tblProcesoDetalleCategoria != null)
                {
                    ProcessID = tblProcesoDetalleCategoria.lProceso_id;

                    if (id > 0)
                    {
                        proceso.ProcesosCategoriaEliminados.Add(tblProcesoDetalleCategoria);
                    }

                    proceso.tblProcesoDetalleCategorias.Remove(tblProcesoDetalleCategoria);
                }
            }

            string url = Url.Action("Index", "tblProcesoDetalleCategorias", new { id = ProcessID });
            return Json(new { success = true, url = url });
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
