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
using Kendo.Mvc.Extensions;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_PostulantesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_Postulantes
        public ActionResult Index()
        {
            var gatbl_Postulantes = db.gatbl_Postulantes;
            return View(gatbl_Postulantes.ToList());
        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var post = from p in db.gatbl_Postulantes.Include(g => g.TipoDocumentoPersonal).Include(g => g.Procedencia).Include(g => g.Departamento).Include(g => g.Nacionalidad).Include(g => g.Responsables)
                       orderby p.lPostulante_id descending
                       select p;
            return Json(post.ToDataSourceResult(request, p => new
            {
                p.lPostulante_id,
                p.dtFecha_registro_dt,
                p.sDocumento_nro,
                p.sNombre_desc,
                p.NombreCompleto,
                p.sDireccion_desc,
                p.sTelefonos_desc,
                p.sMail_desc,
                p.lResponsable_id,
                TipoDocumentoPersonal = new
                {
                    p.lTipoDocumentoPersonal_id,
                    p.TipoDocumentoPersonal.sDescripcion
                },
                Procedencia = new
                {
                    p.lProcedencia_id,
                    p.Nacionalidad.sDescripcion
                },
                Departamento = new
                {
                    p.sDepartamento_fl,
                   sDescripcion = p.sDepartamento_fl != null ? p.Departamento.sDescripcion: ""
                },
                Nacionalidad = new
                {
                    p.lNacionalidad_id,
                    p.Nacionalidad.sGentilicio
                },
                Responsables = new
                {
                    p.lResponsable_id,
                    p.Responsables.NombreCompleto
                }
            }));
        }

        public JsonResult CarreraList(string text)
        {            
            var carreras = from c in db.carreras
                           join f in db.secciones_academicas on c.sca_codigo equals f.sca_codigo
                           where c.crr_descripcion.Contains(text)
                           orderby c.crr_descripcion
                           select new { crr_codigo = c.crr_codigo, crr_descripcion = c.crr_descripcion, sca_codigo = c.sca_codigo, sca_descripcion = f.sca_descripcion};

            return Json(carreras, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerPostulantes(string text)
        {            
            string sql = string.Format("Exec sp_PostulantesLista");

            var post = db.Database.SqlQuery<ViewModels.Postulante>(sql).ToList<ViewModels.Postulante>();


            return Json((from p in post
                         select new
                         {
                             sApPaterno_desc = p.sApPaterno_desc,
                             sApMaterno_desc = p.sApMaterno_desc,
                             sNombre_desc = p.sNombre_desc,
                             sNroRegistro = p.sNroRegistro,
                             sDireccion_desc = p.sDireccion_desc,
                             sTelefonos_desc = p.sTelefonos_desc,
                             sMail_desc = p.sMail_desc,
                             NombreCompleto = p.NombreCompleto
                         }).Where(g => g.NombreCompleto.Contains(text.ToUpper())).OrderBy(g => g.NombreCompleto), JsonRequestBehavior.AllowGet);
        }      

        // GET: gatbl_Postulantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Postulantes gatbl_Postulantes = db.gatbl_Postulantes.Find(id);
            if (gatbl_Postulantes == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Postulantes);
        }

        // GET: gatbl_Postulantes/Create
        public ActionResult Create()
        {
            ViewBag.lTipoDocumentoPersonal_id = new SelectList(db.TipoDocumentoPersonales, "lTipoDocumentoPersonal_id", "sDescripcion");
            ViewBag.lProcedencia_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sDescripcion");
            ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion");
            ViewBag.lNacionalidad_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sGentilicio");

            return View();
        }

        // POST: gatbl_Postulantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_Postulantes gatbl_Postulantes)
        {
            try
            {
                ViewBag.lTipoDocumentoPersonal_id = new SelectList(db.TipoDocumentoPersonales, "lTipoDocumentoPersonal_id", "sDescripcion");
                ViewBag.lProcedencia_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sDescripcion");
                ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion");
                ViewBag.lNacionalidad_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sGentilicio");
                
                
                var Pais = db.Nacionalidades.Find(gatbl_Postulantes.lProcedencia_id);
                if (!Pais.sDescripcion.ToUpper().Equals("BOLIVIA"))
                {
                    gatbl_Postulantes.sDepartamento_fl = null;
                }
                
                

                gatbl_Postulantes.dtFecha_registro_dt = DateTime.Now;
                //gatbl_Postulantes.sca_codigo = carrera.sca_codigo;
                gatbl_Postulantes.lResponsable_id = "0000001138";

                //gatbl_Postulantes.secciones_academicas = facultad;

                gatbl_Postulantes.iEstado_fl = "1";
                gatbl_Postulantes.iEliminado_fl = "1";
                gatbl_Postulantes.sCreated_by = DateTime.Now.ToString();
                gatbl_Postulantes.iConcurrencia_id = 1;

                //if (ModelState.IsValid)
                //{                                                
                //    db.gatbl_Postulantes.Add(gatbl_Postulantes);
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}

                db.gatbl_Postulantes.Add(gatbl_Postulantes);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                //ViewBag.crr_codigo = new SelectList(db.carreras, "crr_codigo", "crr_descripcion", gatbl_Postulantes.crr_codigo);
                //ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion", gatbl_Postulantes.sca_codigo);
                return View(gatbl_Postulantes);
            }                        
        }

        // GET: gatbl_Postulantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Postulantes gatbl_Postulantes = db.gatbl_Postulantes.Find(id);
            if (gatbl_Postulantes == null)
            {
                return HttpNotFound();
            }

            ViewBag.lTipoDocumentoPersonal_id = new SelectList(db.TipoDocumentoPersonales, "lTipoDocumentoPersonal_id", "sDescripcion", gatbl_Postulantes.lTipoDocumentoPersonal_id);
            ViewBag.lProcedencia_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sDescripcion", gatbl_Postulantes.lProcedencia_id);
            ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion", gatbl_Postulantes.sDepartamento_fl);
            ViewBag.lNacionalidad_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sGentilicio", gatbl_Postulantes.lNacionalidad_id);

            return View(gatbl_Postulantes);
        }

        // POST: gatbl_Postulantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_Postulantes gatbl_Postulantes)
        {
            try
            {
                //var carrera = db.carreras.Find(gatbl_Postulantes.crr_codigo);
                //var facultad = db.secciones_academicas.Find(carrera.sca_codigo);

                var Pais = db.Nacionalidades.Find(gatbl_Postulantes.lProcedencia_id);
                if (!Pais.sDescripcion.ToUpper().Equals("BOLIVIA"))
                {
                    gatbl_Postulantes.sDepartamento_fl = null;
                }


                gatbl_Postulantes.dtFecha_registro_dt = DateTime.Now;
                //gatbl_Postulantes.sca_codigo = carrera.sca_codigo;
                gatbl_Postulantes.lResponsable_id = "0000001138";

                //gatbl_Postulantes.secciones_academicas = facultad;

                gatbl_Postulantes.iEstado_fl = "1";
                gatbl_Postulantes.iEliminado_fl = "1";
                gatbl_Postulantes.sCreated_by = DateTime.Now.ToString();
                gatbl_Postulantes.iConcurrencia_id = 1;

                //if (ModelState.IsValid)
                //{
                //    db.Entry(gatbl_Postulantes).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}

                db.Entry(gatbl_Postulantes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.lTipoDocumentoPersonal_id = new SelectList(db.TipoDocumentoPersonales, "lTipoDocumentoPersonal_id", "sDescripcion", gatbl_Postulantes.lTipoDocumentoPersonal_id);
                ViewBag.lProcedencia_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sDescripcion", gatbl_Postulantes.lProcedencia_id);
                ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion", gatbl_Postulantes.sDepartamento_fl);
                ViewBag.lNacionalidad_id = new SelectList(db.Nacionalidades, "lNacionalidad_id", "sGentilicio", gatbl_Postulantes.lNacionalidad_id);

                return View(gatbl_Postulantes);
            }                       
        }

        // GET: gatbl_Postulantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Postulantes gatbl_Postulantes = db.gatbl_Postulantes.Find(id);
            if (gatbl_Postulantes == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Postulantes);
        }

        // POST: gatbl_Postulantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_Postulantes gatbl_Postulantes = db.gatbl_Postulantes.Find(id);
            db.gatbl_Postulantes.Remove(gatbl_Postulantes);
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
