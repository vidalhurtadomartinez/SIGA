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
    public class tblProcesoDetallesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblProcesoDetalles
        public ActionResult Index(int id)
        {
            ViewBag.ProcessID = id;
            
            var tblProcesoDetalle = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalles;

            return PartialView("_Index", tblProcesoDetalle.ToList());            
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.ProcessID = id;
            var tblProcesoDetalle = db.tblProcesoDetalle.Where(a => a.lProceso_id == id);

            return PartialView("_List", tblProcesoDetalle.ToList());
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

        // GET: tblProcesoDetalles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProcesoDetalle tblProcesoDetalle = db.tblProcesoDetalle.Find(id);
            if (tblProcesoDetalle == null)
            {
                return HttpNotFound();
            }
            return View(tblProcesoDetalle);
        }

        // GET: tblProcesoDetalles/Create
        public ActionResult Create(int ProcessID)
        {
            tblProcesoDetalle tblProcesoDetalle = new tblProcesoDetalle();
            tblProcesoDetalle.lProceso_id = ProcessID;


            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo");
            ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre");
            ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion");
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");

            return PartialView("_Create", tblProcesoDetalle);
        }

        // POST: tblProcesoDetalles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblProcesoDetalle tblProcesoDetalle)
        {
            if (ModelState.IsValid)
            {
                //db.tblProcesoDetalle.Add(tblProcesoDetalle);
                //db.SaveChanges();

                //var agenda = db.agenda.Find(tblProcesoDetalle.agd_codigo);
                var personal = db.vVistaPersonal.Find(tblProcesoDetalle.lParticipante_id);                
                var categoria = db.tblCategoria.Find(tblProcesoDetalle.lCategoria_id);
                var rol = db.RolParticipante.Find(tblProcesoDetalle.lRolParticipante_id);

                ViewModels.Proceso proceso = (ViewModels.Proceso)Session["vProceso"];

                int DetalleId = proceso.tblProcesoDetalles.Count() + 1;
                tblProcesoDetalle.lProcesoDetalle_id = -DetalleId;
                tblProcesoDetalle.vVistaPersonal = personal;
                tblProcesoDetalle.tblCategoria = categoria;
                tblProcesoDetalle.RolParticipante = rol;

                proceso.tblProcesoDetalles.Add(tblProcesoDetalle);

                string url = Url.Action("Index", "tblProcesoDetalles", new { id = tblProcesoDetalle.lProceso_id });
                return Json(new { success = true, url = url });
                
            }

            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo", tblProcesoDetalle.agd_codigo);
            ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre", tblProcesoDetalle.lParticipante_id);
            ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion", tblProcesoDetalle.lRolParticipante_id);
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion", tblProcesoDetalle.lCategoria_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblProcesoDetalle.lProceso_id);

            return PartialView("_Create", tblProcesoDetalle);
        }

        // GET: tblProcesoDetalles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            tblProcesoDetalle tblProcesoDetalle = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalles.FirstOrDefault(t => t.lProcesoDetalle_id == id);
            if (tblProcesoDetalle == null)
            {
                return HttpNotFound();
            }



            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo", tblProcesoDetalle.agd_codigo);
            ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre", tblProcesoDetalle.lParticipante_id);
            ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion", tblProcesoDetalle.lRolParticipante_id);
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion", tblProcesoDetalle.lCategoria_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblProcesoDetalle.lProceso_id);

            return PartialView("_Edit", tblProcesoDetalle);
        }

        // POST: tblProcesoDetalles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblProcesoDetalle tblProcesoDetalle)
        {
            if (ModelState.IsValid)
            {               
                tblProcesoDetalle procesoactual = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalles.FirstOrDefault(t => t.lProcesoDetalle_id == tblProcesoDetalle.lProcesoDetalle_id);

                var personal = db.vVistaPersonal.Find(tblProcesoDetalle.lParticipante_id);
                var categoria = db.tblCategoria.Find(tblProcesoDetalle.lCategoria_id);
                var rol = db.RolParticipante.Find(tblProcesoDetalle.lRolParticipante_id);

                if (procesoactual != null)
                {
                    procesoactual.lProcesoDetalle_id = tblProcesoDetalle.lProcesoDetalle_id;
                    //procesoactual.agd_codigo = tblProcesoDetalle.agd_codigo;
                    procesoactual.lParticipante_id = tblProcesoDetalle.lParticipante_id;
                    procesoactual.lCategoria_id = tblProcesoDetalle.lCategoria_id;
                    procesoactual.lRolParticipante_id = tblProcesoDetalle.lRolParticipante_id;
                    procesoactual.lProceso_id = tblProcesoDetalle.lProceso_id;
                    procesoactual.lRevision1 = tblProcesoDetalle.lRevision1;
                    procesoactual.lRevision2 = tblProcesoDetalle.lRevision2;
                    procesoactual.lRevision3 = tblProcesoDetalle.lRevision3;

                    procesoactual.vVistaPersonal = personal;
                    procesoactual.tblCategoria = categoria;
                    procesoactual.RolParticipante = rol;
                }

                //db.Entry(tblProcesoDetalle).State = EntityState.Modified;
                //db.SaveChanges();

                string url = Url.Action("Index", "tblProcesoDetalles", new { id = tblProcesoDetalle.lProceso_id });
                return Json(new { success = true, url = url });
            }
            //ViewBag.agd_codigo = new SelectList(db.agenda, "agd_codigo", "tagd_codigo", tblProcesoDetalle.agd_codigo);
            ViewBag.lParticipante_id = new SelectList(db.vVistaPersonal, "idcont", "Nombre", tblProcesoDetalle.lParticipante_id);
            ViewBag.lRolParticipante_id = new SelectList(db.RolParticipante, "lRolParticipante_id", "sDescripcion", tblProcesoDetalle.lRolParticipante_id);
            ViewBag.lCategoria_id = new SelectList(db.tblCategoria, "lCategoria_id", "sDescripcion", tblProcesoDetalle.lCategoria_id);
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblProcesoDetalle.lProceso_id);

            return PartialView("_Edit", tblProcesoDetalle);
        }

        // GET: tblProcesoDetalles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            tblProcesoDetalle tblProcesoDetalle = ((ViewModels.Proceso)Session["vProceso"]).tblProcesoDetalles.FirstOrDefault(t => t.lProcesoDetalle_id == id);
            if (tblProcesoDetalle == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", tblProcesoDetalle);
        }

        // POST: tblProcesoDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //tblProcesoDetalle tblProcesoDetalle = db.tblProcesoDetalle.Find(id);
            //db.tblProcesoDetalle.Remove(tblProcesoDetalle);
            //db.SaveChanges();
            //return RedirectToAction("Index");

            var proceso = (ViewModels.Proceso)Session["vProceso"];
            int ProcessID = 0;
            if (proceso != null)
            {
                tblProcesoDetalle tblProcesoDetalle = proceso.tblProcesoDetalles.FirstOrDefault(t => t.lProcesoDetalle_id == id);

                if (tblProcesoDetalle != null)
                {
                    ProcessID = tblProcesoDetalle.lProceso_id;

                    if (id > 0)
                    {
                        proceso.ProcesosEliminados.Add(tblProcesoDetalle);
                    }

                    proceso.tblProcesoDetalles.Remove(tblProcesoDetalle);
                }
            }

            string url = Url.Action("Index", "tblProcesoDetalles", new { id = ProcessID });
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
