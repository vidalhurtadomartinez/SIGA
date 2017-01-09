using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CRM.Models;

namespace SIGAA.Areas.CRM.Controllers
{
    public class tblClientesController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblClientes
        public ActionResult Index()
        {
            var tblCliente = db.tblCliente.Include(t => t.tblActividad).Include(t => t.tblCargo).Include(t => t.tblCarrera).Include(t => t.tblTipoColegio).Include(t => t.tblCiudad).Include(t => t.tblCiudadExpedido).Include(t => t.tblColegio).Include(t => t.tblFormaContacto).Include(t => t.tblinstitucion).Include(t => t.tblMedioInformacion).Include(t => t.tblPaisNacimiento).Include(t => t.tblPaisNacionalidad).Include(t => t.tblAsesor).Include(t => t.tblRelacionPostulante).Include(t => t.tblRubroActividad).Include(t => t.tblTipoDocumento).Include(t => t.tblUniversidad);
            return View(tblCliente.ToList());
        }

        // GET: tblClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCliente tblCliente = db.tblCliente.Find(id);
            if (tblCliente == null)
            {
                return HttpNotFound();
            }
            return View(tblCliente);
        }

        public JsonResult ActividadList(int id)
        {
            var actividades = from s in db.tblActividad
                             where s.lRubroActividad_id == id
                             select s;

            return Json(new SelectList(actividades.ToArray(), "lActividad_id", "Descripcion"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarreraList(int id)
        {
            var carreras = from s in db.tblCarrera
                              where s.lUniversidad_id == id
                              select s;

            return Json(new SelectList(carreras.ToArray(), "lCarrera_id", "Descripcion"), JsonRequestBehavior.AllowGet);
        }

        // GET: tblClientes/Create
        public ActionResult Create()
        {
            ViewBag.lActividad_id = new SelectList(db.tblActividad, "lActividad_id", "Descripcion");
            ViewBag.lCargo_id = new SelectList(db.tblCargos, "lCargo_id", "Descripcion");
            ViewBag.lAsesor_id = new SelectList(db.tblAsesor, "lAsesor_id", "NombreCompleto");
            ViewBag.lCarrera_id = new SelectList(db.tblCarrera, "lCarrera_id", "Descripcion");
            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion");
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion");
            ViewBag.lLugarExpedido_id = new SelectList(db.tblCiudad, "lCiudad_id", "Prefijo");
            ViewBag.lColegio_id = new SelectList(db.tblColegio, "lColegio_id", "Descripcion");
            ViewBag.lFormaContacto_id = new SelectList(db.tblFormaContacto, "lFormaContacto_id", "Descripcion");
            ViewBag.lInstitucion_id = new SelectList(db.tblinstitucion, "lInstitucion_id", "Descripcion");
            ViewBag.lMedioInformacion_id = new SelectList(db.tblMedioInformacion, "lMedioInformacion_id", "Descripcion");
            ViewBag.lPaisNacimiento_id = new SelectList(db.tblPais, "lPais_id", "Descripcion");
            ViewBag.lNacionalidad_id = new SelectList(db.tblPais, "lPais_id", "Gentilicio");
            ViewBag.lRelacionPostulante_id = new SelectList(db.tblRelacionPostulante, "lRelacionPostulante_id", "Descripcion");
            ViewBag.lRubro_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion");
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumento, "lTipoDocumento_id", "Descripcion");
            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion");

            return View(new tblCliente() { dtFechaRegistro_dt = DateTime.Now, dtFechaInicioTrabajo_dt = null, dtFechaNacimiento_dt = null});
        }

        // POST: tblClientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCliente tblCliente)
        {
            if (ModelState.IsValid)
            {
                tblCliente.dtFechaRegistro_dt = DateTime.Now;

                db.tblCliente.Add(tblCliente);
                db.SaveChanges();
                return RedirectToAction("Init", "tblEventos", new { id= tblCliente.lCliente_id });
            }

            ViewBag.lActividad_id = new SelectList(db.tblActividad, "lActividad_id", "Descripcion", tblCliente.lActividad_id);
            ViewBag.lCargo_id = new SelectList(db.tblCargos, "lCargo_id", "Descripcion", tblCliente.lCargo_id);
            ViewBag.lAsesor_id = new SelectList(db.tblAsesor, "lAsesor_id", "NombreCompleto", tblCliente.lAsesor_id);
            ViewBag.lCarrera_id = new SelectList(db.tblCarrera, "lCarrera_id", "Descripcion", tblCliente.lCarrera_id);
            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion", tblCliente.lTipoColegio_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblCliente.lCiudad_id);
            ViewBag.lLugarExpedido_id = new SelectList(db.tblCiudad, "lCiudad_id", "Prefijo", tblCliente.lLugarExpedido_id);
            ViewBag.lColegio_id = new SelectList(db.tblColegio, "lColegio_id", "Descripcion", tblCliente.lColegio_id);
            ViewBag.lFormaContacto_id = new SelectList(db.tblFormaContacto, "lFormaContacto_id", "Descripcion", tblCliente.lFormaContacto_id);
            ViewBag.lInstitucion_id = new SelectList(db.tblinstitucion, "lInstitucion_id", "Descripcion", tblCliente.lInstitucion_id);
            ViewBag.lMedioInformacion_id = new SelectList(db.tblMedioInformacion, "lMedioInformacion_id", "Descripcion", tblCliente.lMedioInformacion_id);
            ViewBag.lPaisNacimiento_id = new SelectList(db.tblPais, "lPais_id", "Descripcion", tblCliente.lPaisNacimiento_id);
            ViewBag.lNacionalidad_id = new SelectList(db.tblPais, "lPais_id", "Gentilicio", tblCliente.lNacionalidad_id);
            ViewBag.lRelacionPostulante_id = new SelectList(db.tblRelacionPostulante, "lRelacionPostulante_id", "Descripcion", tblCliente.lRelacionPostulante_id);
            ViewBag.lRubro_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion", tblCliente.lRubro_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumento, "lTipoDocumento_id", "Descripcion", tblCliente.lTipoDocumento_id);
            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion", tblCliente.lUniversidad_id);
            return View(tblCliente);
        }

        // GET: tblClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCliente tblCliente = db.tblCliente.Find(id);
            if (tblCliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.lActividad_id = new SelectList(db.tblActividad.Where(a=>a.lRubroActividad_id == tblCliente.lRubro_id), "lActividad_id", "Descripcion", tblCliente.lActividad_id);
            ViewBag.lCargo_id = new SelectList(db.tblCargos, "lCargo_id", "Descripcion", tblCliente.lCargo_id);
            ViewBag.lAsesor_id = new SelectList(db.tblAsesor, "lAsesor_id", "NombreCompleto", tblCliente.lAsesor_id);
            ViewBag.lCarrera_id = new SelectList(db.tblCarrera.Where(c=> c.lUniversidad_id == tblCliente.lUniversidad_id), "lCarrera_id", "Descripcion", tblCliente.lCarrera_id);
            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion", tblCliente.lTipoColegio_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblCliente.lCiudad_id);
            ViewBag.lLugarExpedido_id = new SelectList(db.tblCiudad, "lCiudad_id", "Prefijo", tblCliente.lLugarExpedido_id);
            ViewBag.lColegio_id = new SelectList(db.tblColegio, "lColegio_id", "Descripcion", tblCliente.lColegio_id);
            ViewBag.lFormaContacto_id = new SelectList(db.tblFormaContacto, "lFormaContacto_id", "Descripcion", tblCliente.lFormaContacto_id);
            ViewBag.lInstitucion_id = new SelectList(db.tblinstitucion, "lInstitucion_id", "Descripcion", tblCliente.lInstitucion_id);
            ViewBag.lMedioInformacion_id = new SelectList(db.tblMedioInformacion, "lMedioInformacion_id", "Descripcion", tblCliente.lMedioInformacion_id);
            ViewBag.lPaisNacimiento_id = new SelectList(db.tblPais, "lPais_id", "Descripcion", tblCliente.lPaisNacimiento_id);
            ViewBag.lNacionalidad_id = new SelectList(db.tblPais, "lPais_id", "Gentilicio", tblCliente.lNacionalidad_id);
            ViewBag.lRelacionPostulante_id = new SelectList(db.tblRelacionPostulante, "lRelacionPostulante_id", "Descripcion", tblCliente.lRelacionPostulante_id);
            ViewBag.lRubro_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion", tblCliente.lRubro_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumento, "lTipoDocumento_id", "Descripcion", tblCliente.lTipoDocumento_id);
            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion", tblCliente.lUniversidad_id);
            return View(tblCliente);
        }

        // POST: tblClientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCliente tblCliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lActividad_id = new SelectList(db.tblActividad.Where(a => a.lRubroActividad_id == tblCliente.lRubro_id), "lActividad_id", "Descripcion", tblCliente.lActividad_id);
            ViewBag.lCargo_id = new SelectList(db.tblCargos, "lCargo_id", "Descripcion", tblCliente.lCargo_id);
            ViewBag.lAsesor_id = new SelectList(db.tblAsesor, "lAsesor_id", "NombreCompleto", tblCliente.lAsesor_id);
            ViewBag.lCarrera_id = new SelectList(db.tblCarrera.Where(c => c.lUniversidad_id == tblCliente.lUniversidad_id), "lCarrera_id", "Descripcion", tblCliente.lCarrera_id);
            ViewBag.lTipoColegio_id = new SelectList(db.tblTipoColegio, "lTipoColegio_id", "Descripcion", tblCliente.lTipoColegio_id);
            ViewBag.lCiudad_id = new SelectList(db.tblCiudad, "lCiudad_id", "Descripcion", tblCliente.lCiudad_id);
            ViewBag.lLugarExpedido_id = new SelectList(db.tblCiudad, "lCiudad_id", "Prefijo", tblCliente.lLugarExpedido_id);
            ViewBag.lColegio_id = new SelectList(db.tblColegio, "lColegio_id", "Descripcion", tblCliente.lColegio_id);
            ViewBag.lFormaContacto_id = new SelectList(db.tblFormaContacto, "lFormaContacto_id", "Descripcion", tblCliente.lFormaContacto_id);
            ViewBag.lInstitucion_id = new SelectList(db.tblinstitucion, "lInstitucion_id", "Descripcion", tblCliente.lInstitucion_id);
            ViewBag.lMedioInformacion_id = new SelectList(db.tblMedioInformacion, "lMedioInformacion_id", "Descripcion", tblCliente.lMedioInformacion_id);
            ViewBag.lPaisNacimiento_id = new SelectList(db.tblPais, "lPais_id", "Descripcion", tblCliente.lPaisNacimiento_id);
            ViewBag.lNacionalidad_id = new SelectList(db.tblPais, "lPais_id", "Gentilicio", tblCliente.lNacionalidad_id);
            ViewBag.lRelacionPostulante_id = new SelectList(db.tblRelacionPostulante, "lRelacionPostulante_id", "Descripcion", tblCliente.lRelacionPostulante_id);
            ViewBag.lRubro_id = new SelectList(db.tblRubroActividad, "lRubroActividad_id", "Descripcion", tblCliente.lRubro_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumento, "lTipoDocumento_id", "Descripcion", tblCliente.lTipoDocumento_id);
            ViewBag.lUniversidad_id = new SelectList(db.tblUniversidad, "lUniversidad_id", "Descripcion", tblCliente.lUniversidad_id);
            return View(tblCliente);
        }

        // GET: tblClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCliente tblCliente = db.tblCliente.Find(id);
            if (tblCliente == null)
            {
                return HttpNotFound();
            }
            return View(tblCliente);
        }

        // POST: tblClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCliente tblCliente = db.tblCliente.Find(id);
            db.tblCliente.Remove(tblCliente);
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
