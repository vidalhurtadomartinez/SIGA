using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_PConvalidacionesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_PConvalidaciones
        public ActionResult Index()
        {
            //ViewBag.crr_codigo = new SelectList(db.carreras, "crr_codigo", "crr_descripcion");
            var gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Include(g => g.Responsables);
            return View(db.gatbl_PConvalidaciones.ToList());
        }

        public ActionResult ServerFiltering()
        {
            return View();
        }

        public JsonResult GetProducts()
        {           
            return Json(db.carreras, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPostulantes(string text)
        {
            //var postulantes = db.agendas.Select(agenda => new agenda
            //{
            //    agd_nombres = agenda.agd_appaterno.Trim() + " " + agenda.agd_apmaterno.Trim() +
            //    ", " + agenda.agd_nombres.Trim(),
            //    agd_codigo = agenda.agd_codigo.Trim(),
            //    agd_docnro = agenda.agd_docnro.Trim()
            //});

            //if (!string.IsNullOrEmpty(text))
            //{
            //    postulantes = (from ag in db.agendas
            //                  join alg in db.alumnos_agenda on ag.agd_codigo equals alg.agd_codigo
            //            select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres);
            //}            

            //return Json(postulantes, JsonRequestBehavior.AllowGet);

            return Json((from ag in db.agendas
                         join alg in db.alumnos_agenda on ag.agd_codigo equals alg.agd_codigo
                         //orderby ag.agd_nombres ascending
                         select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerResponsables(string text)
        {            
            return Json((from ag in db.agendas
                         select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ObtenerPostulantes()
        //{
        //    return Json((from ag in db.agendas
        //                join alg in db.alumnos_agenda on ag.agd_codigo equals alg.agd_codigo
        //                //orderby ag.agd_nombres ascending
        //                select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        //    //return Json(db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim(), agd_docnro = g.agd_docnro.Trim() }).Where(g => g.agd_codigo.Any(db.alumnos_agenda.Select(al => new { alagd_codigo = al.agd_codigo.Trim() }))).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult ObtenerCarreras()
        {
            return Json(db.carreras.Select(g => new { crr_descripcion = g.crr_descripcion.Trim(), crr_codigo = g.crr_codigo.Trim(), sca_codigo = g.sca_codigo }).OrderBy(g => g.crr_descripcion), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetCarreras(string term)
        //{
        //    return this.Json(db.carreras.Where(t => t.crr_descripcion.Contains(term)),
        //                    JsonRequestBehavior.AllowGet);
        //}

        // GET: gatbl_PConvalidaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            if (gatbl_PConvalidaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_PConvalidaciones);
        }

        // GET: gatbl_PConvalidaciones/Create
        public ActionResult Create()
        {
            ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);
            //ViewBag.agd_codigo = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);
            //ViewBag.crr_codigo = db.carreras.Select(g => new { crr_descripcion = g.crr_descripcion.Trim(), crr_codigo = g.crr_codigo.Trim() }).OrderBy(g => g.crr_descripcion);

            ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion");
            //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "sca_descripcion");

            var model = new gatbl_PConvalidaciones();
            return View(model);
        }

        // POST: gatbl_PConvalidaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_PConvalidaciones gatbl_PConvalidaciones)
        {            
            if (ModelState.IsValid)
            {
                gatbl_PConvalidaciones.iEstado_fl = "1";
                gatbl_PConvalidaciones.iEliminado_fl = "1";
                gatbl_PConvalidaciones.sCreated_by = DateTime.Now.ToString();
                gatbl_PConvalidaciones.iConcurrencia_id = 1;

                db.gatbl_PConvalidaciones.Add(gatbl_PConvalidaciones);
                db.SaveChanges();


                var preconvalidacion = new gatbl_AnalisisPreConvalidaciones()
                {
                    lPConvalidacion_id = gatbl_PConvalidaciones.lPConvalidacion_id,
                    lResponsable_id = "0000001138",
                    sVersion_nro = 1,
                    dtAnalisisConvalidacion_dt = DateTime.Now,
                    sObs_desc = string.Empty,
                    iEstado_fl = "2",
                    iEliminado_fl = "1",
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1                    
                };

                db.gatbl_AnalisisPreConvalidaciones.Add(preconvalidacion);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion");
            ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);

            return View(gatbl_PConvalidaciones);
        }

        // GET: gatbl_PConvalidaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            if (gatbl_PConvalidaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_PConvalidaciones);
        }

        // POST: gatbl_PConvalidaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lPConvalidacion_id,sGestion_desc,sPeriodo_desc,lPostulante_id,lFacultad_id,lCarrera_id,agd_codigo,sca_codigo,crr_codigo,sAgenda_nro,sDocumento_nro,dtPostulacion_dt,lResponsable_id,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_PConvalidaciones gatbl_PConvalidaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatbl_PConvalidaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gatbl_PConvalidaciones);
        }

        // GET: gatbl_PConvalidaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            if (gatbl_PConvalidaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_PConvalidaciones);
        }

        // POST: gatbl_PConvalidaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            db.gatbl_PConvalidaciones.Remove(gatbl_PConvalidaciones);
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
