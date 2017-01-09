using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
using Kendo.Mvc.UI;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_ConvalidacionesExternasPAController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_ConvalidacionesExternasPA
        public ActionResult Index()
        {
            var gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Include(g => g.agenda).Include(g => g.Responsable);
            return View(gatbl_ConvalidacionesExternasPA.ToList());
        }

        public JsonResult ObtenerPostulantes(string text)
        {
            gatbl_ConvalidacionesExternasPA convalidacionExterna = Session["convalidacionExterna"] as gatbl_ConvalidacionesExternasPA;

            if (string.IsNullOrEmpty(text) && convalidacionExterna.lEstudiante_id != null)
            {
                return Json((from p in db.gatbl_Postulantes
                             join es in db.gatbl_PConvalidaciones on p.lPostulante_id equals es.lPostulante_id
                             //join ca in db.carreras on p.crr_codigo equals ca.crr_codigo
                             //join sa in db.secciones_academicas on ca.sca_codigo equals sa.sca_codigo
                             where p.sDocumento_nro == convalidacionExterna.lEstudiante_id
                             orderby p.sNombre_desc ascending
                             select new
                             {
                                 sNombre_desc = p.sNombre_desc,
                                 agd_docnro = p.sDocumento_nro
                                 //,
                                 //crr_descripcion = ca.crr_descripcion,
                                 //sca_descripcion = sa.sca_descripcion
                             }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from p in db.gatbl_Postulantes
                             join es in db.gatbl_PConvalidaciones on p.lPostulante_id equals es.lPostulante_id
                             //join ca in db.carreras on p.crr_codigo equals ca.crr_codigo
                             //join sa in db.secciones_academicas on ca.sca_codigo equals sa.sca_codigo
                             where p.sNombre_desc.Contains(text)
                             orderby p.sNombre_desc ascending
                             select new
                             {
                                 sNombre_desc = p.sNombre_desc,
                                 agd_docnro = p.sDocumento_nro
                                 //,
                                 //crr_descripcion = ca.crr_descripcion,
                                 //sca_descripcion = sa.sca_descripcion
                             }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerResponsables(string text)
        {
            gatbl_ConvalidacionesExternasPA convalidacionExterna = Session["convalidacionExterna"] as gatbl_ConvalidacionesExternasPA;

            if (string.IsNullOrEmpty(text) && convalidacionExterna.lResponsable_id != null)
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(convalidacionExterna.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }            
        }

        public ActionResult EditingInline_Read([DataSourceRequest] DataSourceRequest request)
        {
            //return Json(productService.Read().ToDataSourceResult(request));
            return Json(db.gatbl_DConvalidacionesExternasPA.ToList(), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, gatbl_MateriasConvalidadas materias)
        {
            //var mat = new gatbl_MateriasConvalidadas();
            //if (materias != null && ModelState.IsValid)
            //{
            //    productService.Create(product);
            //}

            //return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            return Json(new gatbl_MateriasConvalidadas(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HierarchyBinding_Universidades([DataSourceRequest] DataSourceRequest request)
        {
            //return Json(GetEmployees().ToDataSourceResult(request));
            return Json(db.gatbl_DConvalidacionesExternasPA.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HierarchyBinding_Materias(int id, [DataSourceRequest] DataSourceRequest request)
        {
            //return Json(GetOrders()
            //    .Where(order => order.EmployeeID == employeeID)
            //    .ToDataSourceResult(request));
            return Json(db.gatbl_MateriasConvalidadas.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: gatbl_ConvalidacionesExternasPA/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            if (gatbl_ConvalidacionesExternasPA == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_ConvalidacionesExternasPA);
        }

        // GET: gatbl_ConvalidacionesExternasPA/Create
        public ActionResult Create()
        {
            ViewBag.lEstudiante_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            Session["convalidacionExterna"] = new gatbl_ConvalidacionesExternasPA();

            return View();
        }

        // POST: gatbl_ConvalidacionesExternasPA/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lConvalidacionExternaPA_id,lEstudiante_id,dtConvalidacionExterna_dt,sFormulario_nro,sInforme_nro,lResponsable_id,sObs_desc,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA)
        {
            if (ModelState.IsValid)
            {
                db.gatbl_ConvalidacionesExternasPA.Add(gatbl_ConvalidacionesExternasPA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lEstudiante_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ConvalidacionesExternasPA.lEstudiante_id);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ConvalidacionesExternasPA.lResponsable_id);
            return View(gatbl_ConvalidacionesExternasPA);
        }

        // GET: gatbl_ConvalidacionesExternasPA/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            if (gatbl_ConvalidacionesExternasPA == null)
            {
                return HttpNotFound();
            }
            ViewBag.lEstudiante_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ConvalidacionesExternasPA.lEstudiante_id);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ConvalidacionesExternasPA.lResponsable_id);
            return View(gatbl_ConvalidacionesExternasPA);
        }

        // POST: gatbl_ConvalidacionesExternasPA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lConvalidacionExternaPA_id,lEstudiante_id,dtConvalidacionExterna_dt,sFormulario_nro,sInforme_nro,lResponsable_id,sObs_desc,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatbl_ConvalidacionesExternasPA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lEstudiante_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ConvalidacionesExternasPA.lEstudiante_id);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ConvalidacionesExternasPA.lResponsable_id);
            return View(gatbl_ConvalidacionesExternasPA);
        }

        // GET: gatbl_ConvalidacionesExternasPA/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            if (gatbl_ConvalidacionesExternasPA == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_ConvalidacionesExternasPA);
        }

        // POST: gatbl_ConvalidacionesExternasPA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            db.gatbl_ConvalidacionesExternasPA.Remove(gatbl_ConvalidacionesExternasPA);
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
