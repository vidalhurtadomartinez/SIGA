using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_ProgramasAnaliticosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_ProgramasAnaliticos
        public ActionResult Index()
        {
            var gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Include(g => g.gatbl_Carreras).Include(g => g.gatbl_Facultades).Include(g => g.gatbl_Universidades).Include(g => g.Responsables).Include(g => g.UnidadNegocios);
            return View(gatbl_ProgramasAnaliticos.ToList());
        }

        // GET: gatbl_ProgramasAnaliticos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            if (gatbl_ProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_ProgramasAnaliticos);
        }

        // GET: gatbl_ProgramasAnaliticos/Create
        public ActionResult Create()
        {
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.sNivel_fl = new SelectList(db.NivelProgramaAnaliticos, "sNivel_fl", "sDescripcion");
            ViewBag.sOrigen_fl = new SelectList(db.OrigenProgramaAnaliticos, "sOrigen_fl", "sDescripcion");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion");
            return View();
        }

        // POST: gatbl_ProgramasAnaliticos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lProgramaAnalitico_id,sGestion_desc,sPeriodo_desc,dtRegistro_dt,lMateria_id,sMateria_desc,lUNegocio_id,lUniversidad_id,lFacultad_id,lCarrera_id,sNivel_fl,sOrigen_fl,lInstitucion_id,sInstitucion_desc,sCodigo_nro,sSigla_desc,sHorasPracticas_nro,sHorasTeoricas_nro,sHorasSociales_nro,sHorasAyudantia_nro,sCreditos_nro,lResponsable_id,sVersion_nro,sObs_desc,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos)
        {
            if (ModelState.IsValid)
            {
                db.gatbl_ProgramasAnaliticos.Add(gatbl_ProgramasAnaliticos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_ProgramasAnaliticos.lCarrera_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm", gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", gatbl_ProgramasAnaliticos.lUNegocio_id);
            return View(gatbl_ProgramasAnaliticos);
        }

        // GET: gatbl_ProgramasAnaliticos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            if (gatbl_ProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_ProgramasAnaliticos.lCarrera_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm", gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", gatbl_ProgramasAnaliticos.lUNegocio_id);

            return View(gatbl_ProgramasAnaliticos);
        }

        // POST: gatbl_ProgramasAnaliticos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lProgramaAnalitico_id,sGestion_desc,sPeriodo_desc,dtRegistro_dt,lMateria_id,sMateria_desc,lUNegocio_id,lUniversidad_id,lFacultad_id,lCarrera_id,sNivel_fl,sOrigen_fl,lInstitucion_id,sInstitucion_desc,sCodigo_nro,sSigla_desc,sHorasPracticas_nro,sHorasTeoricas_nro,sHorasSociales_nro,sHorasAyudantia_nro,sCreditos_nro,lResponsable_id,sVersion_nro,sObs_desc,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatbl_ProgramasAnaliticos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_ProgramasAnaliticos.lCarrera_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm", gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", gatbl_ProgramasAnaliticos.lUNegocio_id);
            return View(gatbl_ProgramasAnaliticos);
        }

        // GET: gatbl_ProgramasAnaliticos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            if (gatbl_ProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_ProgramasAnaliticos);
        }

        // POST: gatbl_ProgramasAnaliticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            db.gatbl_ProgramasAnaliticos.Remove(gatbl_ProgramasAnaliticos);
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
