using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_FacultadesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_Facultades
        public ActionResult Index()
        {
            var gatbl_Facultades = db.gatbl_Facultades.Include(g => g.gatbl_Universidades);
            return View(gatbl_Facultades.ToList());
        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var fac = from f in db.gatbl_Facultades.Include(g => g.gatbl_Universidades)
                      select f;
            return Json(fac.ToDataSourceResult(request, f => new
            {
                f.lFacultad_id,
                f.lUniversidad_id,
                f.sFacultad_nm,
                f.sResponsable_nm,
                f.sTelefono_desc,
                f.sMail_desc,                              
                gatbl_Universidades = new
                {
                    f.lUniversidad_id,
                    f.gatbl_Universidades.sNombre_desc,
                    f.gatbl_Universidades.sDireccion_desc
                }                
            }));
        }

        // GET: gatbl_Facultades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Facultades gatbl_Facultades = db.gatbl_Facultades.Find(id);
            if (gatbl_Facultades == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Facultades);
        }

        // GET: gatbl_Facultades/Create
        public ActionResult Create()
        {
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            return View();
        }

        // POST: gatbl_Facultades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_Facultades gatbl_Facultad)
        {
            if (ModelState.IsValid)
            {
                gatbl_Facultad.iEstado_fl = "1";
                gatbl_Facultad.iEliminado_fl = "1";
                gatbl_Facultad.sCreated_by = DateTime.Now.ToString();
                gatbl_Facultad.iConcurrencia_id = 1;

                db.gatbl_Facultades.Add(gatbl_Facultad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_Facultad.lUniversidad_id);
            return View(gatbl_Facultad);
        }

        // GET: gatbl_Facultades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Facultades gatbl_Facultades = db.gatbl_Facultades.Find(id);
            if (gatbl_Facultades == null)
            {
                return HttpNotFound();
            }
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_Facultades.lUniversidad_id);
            return View(gatbl_Facultades);
        }

        // POST: gatbl_Facultades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_Facultades gatbl_Facultad)
        {
            if (ModelState.IsValid)
            {
                gatbl_Facultad.iEstado_fl = "1";
                gatbl_Facultad.iEliminado_fl = "1";
                gatbl_Facultad.sCreated_by = DateTime.Now.ToString();
                gatbl_Facultad.iConcurrencia_id = 1;

                db.Entry(gatbl_Facultad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_Facultad.lUniversidad_id);
            return View(gatbl_Facultad);
        }

        // GET: gatbl_Facultades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Facultades gatbl_Facultades = db.gatbl_Facultades.Find(id);
            if (gatbl_Facultades == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Facultades);
        }

        // POST: gatbl_Facultades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_Facultades gatbl_Facultades = db.gatbl_Facultades.Find(id);
            db.gatbl_Facultades.Remove(gatbl_Facultades);
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
