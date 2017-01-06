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

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_CarrerasController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_Carreras
        public ActionResult Index()
        {
            var gatbl_Carreras = db.gatbl_Carreras.Include(g => g.gatbl_Facultades).Include(g => g.gatbl_Universidades);
          
            return View(gatbl_Carreras.ToList().Where(fac => fac.iEstado_fl == "1"));
        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var car = from c in db.gatbl_Carreras.Include(g => g.gatbl_Facultades).Include(g => g.gatbl_Universidades)
                      select c;
            return Json(car.ToDataSourceResult(request, c => new
            {
                c.lCarrera_id,
                c.lFacultad_id,
                c.lUniversidad_id,
                c.sCarrera_nm,
                c.sResponsable_nm,
                c.sTelefono_desc,
                c.sMail_desc,
                gatbl_Universidades = new
                {
                    c.lUniversidad_id,
                    c.gatbl_Universidades.sNombre_desc,
                    c.gatbl_Universidades.sDireccion_desc
                },
                gatbl_Facultades = new
                {
                    c.lUniversidad_id,
                    c.lFacultad_id,
                    c.gatbl_Facultades.sFacultad_nm,
                    c.gatbl_Facultades.sMail_desc
                }
            }));
        }

        // GET: gatbl_Carreras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Carreras gatbl_Carreras = db.gatbl_Carreras.Find(id);
            if (gatbl_Carreras == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Carreras);
        }

        public JsonResult SelectFacultad(int id)
        {
            IEnumerable<gatbl_Facultades> Facultades = db.gatbl_Facultades.Where(fac => fac.lUniversidad_id == id);
            return Json(Facultades);
        }

        public ActionResult FillFacultad(int id)
        {
            var facultades = db.gatbl_Facultades.Where(c => c.lUniversidad_id == id);
            return Json(facultades, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FacultadList(int id)
        {
            var facultades = from s in db.gatbl_Facultades
                        where s.lUniversidad_id == id
                        select s;

            return Json(new SelectList(facultades.ToArray(), "lFacultad_id", "sFacultad_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult States(int id)
        {
            List<string> facultades = new List<string>();
            switch (id)
            {
                case 1:
                    facultades.Add("New Delhi");
                    facultades.Add("Mumbai");
                    facultades.Add("Kolkata");
                    facultades.Add("Chennai");
                    break;
                case 2:
                    facultades.Add("Canberra");
                    facultades.Add("Melbourne");
                    facultades.Add("Perth");
                    facultades.Add("Sydney");
                    break;
                case 3:
                    facultades.Add("California");
                    facultades.Add("Florida");
                    facultades.Add("New York");
                    facultades.Add("Washignton");
                    break;
                case 4:
                    facultades.Add("Tunisia");
                    facultades.Add("Libya");
                    facultades.Add("Morocco");
                    facultades.Add("Sudan");
                    break;
            }
            return Json(facultades);
        }

        // GET: gatbl_Carreras/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");

            ViewBag.UniversidadList = db.gatbl_Universidades;
            var model = new gatbl_Carreras();
            return View(model);

            //return View();
        }

        // POST: gatbl_Carreras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_Carreras gatbl_Carrera)
        {
            if (ModelState.IsValid)
            {
                gatbl_Carrera.iEstado_fl = "1";
                gatbl_Carrera.iEliminado_fl = "1";
                gatbl_Carrera.sCreated_by = DateTime.Now.ToString();
                gatbl_Carrera.iConcurrencia_id = 1;

                db.gatbl_Carreras.Add(gatbl_Carrera);
                db.SaveChanges();

                var carrera = db.gatbl_Carreras.Find(gatbl_Carrera.lCarrera_id);

                var Pens = new Pensum();

                Pens.sDescripcion = "Pensum 1";
                Pens.lCarrera_id = carrera.lCarrera_id;
                Pens.lUniversidad_id = carrera.lUniversidad_id;
                Pens.iEstado_fl = "1";
                Pens.iEliminado_fl = "1";
                Pens.sCreated_by = DateTime.Now.ToString();
                Pens.iConcurrencia_id = 1;
                Pens.bActivo = true;

                db.Pensums.Add(Pens);
                db.SaveChanges();
 
                return RedirectToAction("Index");
            }

            //ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm", gatbl_Carrera.lFacultad_id);
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_Carrera.lUniversidad_id);

            ViewBag.UniversidadList = db.gatbl_Universidades;
            
            return View(gatbl_Carrera);
        }

        // GET: gatbl_Carreras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Carreras gatbl_Carreras = db.gatbl_Carreras.Find(id);
            if (gatbl_Carreras == null)
            {
                return HttpNotFound();
            }
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(c => c.lUniversidad_id == gatbl_Carreras.lUniversidad_id), "lFacultad_id", "sFacultad_nm", gatbl_Carreras.lFacultad_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_Carreras.lUniversidad_id);

            //ViewBag.UniversidadList = db.gatbl_Universidades;
            //ViewBag.FacultadList = db.gatbl_Facultades.Where(c => c.lUniversidad_id == id);

            return View(gatbl_Carreras);
        }

        // POST: gatbl_Carreras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_Carreras gatbl_Carrera)
        {
            if (ModelState.IsValid)
            {
                gatbl_Carrera.iEstado_fl = "1";
                gatbl_Carrera.iEliminado_fl = "1";
                gatbl_Carrera.sCreated_by = DateTime.Now.ToString();
                gatbl_Carrera.iConcurrencia_id = 1;

                db.Entry(gatbl_Carrera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm", gatbl_Carrera.lFacultad_id);
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_Carrera.lUniversidad_id);            

            return View(gatbl_Carrera);
        }

        // GET: gatbl_Carreras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Carreras gatbl_Carreras = db.gatbl_Carreras.Find(id);
            if (gatbl_Carreras == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Carreras);
        }

        // POST: gatbl_Carreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_Carreras gatbl_Carreras = db.gatbl_Carreras.Find(id);
            db.gatbl_Carreras.Remove(gatbl_Carreras);
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
