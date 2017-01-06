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
    public class gatbl_UniversidadesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_Universidades
        public ActionResult Index()
        {
            var gatbl_Universidades = db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad);

            //ViewData["Universidades"] = gatbl_Universidades;

            ViewData["OrigenOtraUniversidad"] = db.OrigenOtraUniversidades;
            ViewData["Departamento"] = db.Departamentos;

            return View(gatbl_Universidades.ToList());         
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gatbl_Universidades_Create([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<gatbl_Universidades> gatbl_Universidades)
        {
            var results = new List<gatbl_Universidades>();
            if (gatbl_Universidades != null && ModelState.IsValid)
            {
                foreach (var univ in gatbl_Universidades)
                {
                    //productService.Create(product);
                    //results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        public IEnumerable<gatbl_Universidades> GetUniversidades()
        {            
            return db.gatbl_Universidades.Select(universidad => new gatbl_Universidades
            {
                lUniversidad_id = universidad.lUniversidad_id,
                sOrigen_fl = universidad.sOrigen_fl,
                sDepartamento_fl = universidad.sDepartamento_fl,
                sNombre_desc = universidad.sNombre_desc,
                sDireccion_desc = universidad.sDireccion_desc,
                sTelefonos_desc = universidad.sTelefonos_desc
            });
        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var uni = from u in db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad)
                      select u;
            //select new {
            //    lUniversidad_id = u.lUniversidad_id,
            //    sOrigen_fl = u.sOrigen_fl,
            //    sDepartamento_fl = u.sDepartamento_fl,
            //    sNombre_desc = u.sNombre_desc,
            //    sDireccion_desc = u.sDireccion_desc,
            //    sTelefonos_desc = u.sTelefonos_desc};

            //var uni = db.gatbl_Universidades;

            return Json(uni.ToDataSourceResult(request, u => new
            {
                u.lUniversidad_id,
                u.sOrigen_fl,  
                OrigenOtraUniversidad = new
                {
                    u.OrigenOtraUniversidad.sOrigen_fl,
                    u.OrigenOtraUniversidad.sDescripcion
                },                             
                u.sDepartamento_fl,
                Departamento = new
                {
                    u.sDepartamento_fl,
                    u.Departamento.sDescripcion
                },
                u.sNombre_desc,
                u.sDireccion_desc,
                u.sTelefonos_desc,
                u.bInterno
            }));

            //return Json(uni.ToDataSourceResult(request, u => new {
            //    lUniversidad_id = u.lUniversidad_id,
            //    sOrigen_fl = u.sOrigen_fl,
            //    sDepartamento_fl = u.sDepartamento_fl,
            //    sNombre_desc = u.sNombre_desc,
            //    sDireccion_desc = u.sDireccion_desc,
            //    sTelefonos_desc = u.sTelefonos_desc,
            //    bInterno = u.bInterno
            //}));
        }

        //public ActionResult Products_Read([DataSourceRequest]DataSourceRequest request)
        //{            
        //    IQueryable<gatbl_Universidades> products = db.gatbl_Universidades;
        //    DataSourceResult result = products.ToDataSourceResult(request);
        //    return Json(result);            
        //}

        //public ActionResult Index(string filter = null, int page = 1,
        // int pageSize = 10, string sort = "sNombre_desc", string sortdir = "ASC")
        //{
        //    //var gatbl_Universidades = db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad);
        //    //return View(gatbl_Universidades.ToList());

        //    var records = new PagedList<gatbl_Universidades>();
        //    ViewBag.filter = filter;

        //    if (filter != null && filter != string.Empty)
        //    {
        //        records.Content = db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad)
        //                     .Where(x => filter == null ||
        //                        (x.sNombre_desc.Contains(filter))
        //                           || x.sDireccion_desc.Contains(filter)
        //                           || x.OrigenOtraUniversidad.sDescripcion.Contains(filter)
        //                           || x.Departamento.sDescripcion.Contains(filter)
        //                           || x.sTelefonos_desc.Contains(filter)
        //                      )
        //                .OrderBy(u => u.sNombre_desc)
        //                .Skip((page - 1) * pageSize)
        //                .Take(pageSize)
        //                .ToList();
        //    }
        //    else
        //    {
        //        records.Content = db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad).ToList();
        //    }

        //    records.TotalRecords = records.Content.Count();

        //    records.CurrentPage = page;
        //    records.PageSize = pageSize;

        //    return View(records);
        //}

        public ActionResult Universidad_Read([DataSourceRequest] DataSourceRequest request)
        {
            //var gatbl_Universidades = from u in db.gatbl_Universidades
            //                  select new
            //                  {
            //                      lUniversidad_id = u.lUniversidad_id,
            //                      sOrigen_fl = u.sOrigen_fl,
            //                      sDepartamento_fl = u.sDepartamento_fl,
            //                      sNombre_desc = u.sNombre_desc,
            //                      sDireccion_desc = u.sDireccion_desc,
            //                      sTelefonos_desc = u.sTelefonos_desc
            //                  };
            //var gatbl_Universidades = db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad);
            //DataSourceResult results = gatbl_Universidades.ToDataSourceResult(request);
            //return Json(results);

            IQueryable<gatbl_Universidades> gatbl_Universidades = db.gatbl_Universidades;
            DataSourceResult result = gatbl_Universidades.ToDataSourceResult(request);
            return Json(result);
        }

        //public ActionResult Universidad_Read()
        //{
        //    //var universidad = from u in db.gatbl_Universidades                              
        //    //                  select new { lUniversidad_id = u.lUniversidad_id, sOrigen_fl = u.sOrigen_fl,
        //    //                      sDepartamento_fl = u.sDepartamento_fl,
        //    //                      sNombre_desc = u.sNombre_desc,
        //    //                      sDireccion_desc = u.sDireccion_desc,
        //    //                      sTelefonos_desc = u.sTelefonos_desc};

        //    //return Json(universidad, JsonRequestBehavior.AllowGet);

        //    var gatbl_Universidades = db.gatbl_Universidades.Include(g => g.Departamento).Include(g => g.OrigenOtraUniversidad);
        //    return View(gatbl_Universidades.ToList());
        //}

        // GET: gatbl_Universidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Universidades gatbl_Universidades = db.gatbl_Universidades.Find(id);
            if (gatbl_Universidades == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Universidades);
        }

        // GET: gatbl_Universidades/Create
        public ActionResult Create()
        {
            ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion");
            ViewBag.sOrigen_fl = new SelectList(db.OrigenOtraUniversidades, "sOrigen_fl", "sDescripcion");
            ViewData["OrigenOtraUniversidad"] = db.OrigenOtraUniversidades;
            ViewData["Departamento"] = db.Departamentos;

            return View();            
        }

        // POST: gatbl_Universidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Create(gatbl_Universidades gatbl_Universidad)
        {
            if (ModelState.IsValid)
            {
                gatbl_Universidad.iEstado_fl = "1";
                gatbl_Universidad.iEliminado_fl = "1";
                gatbl_Universidad.sCreated_by = DateTime.Now.ToString();
                gatbl_Universidad.iConcurrencia_id = 1;

                db.gatbl_Universidades.Add(gatbl_Universidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion", gatbl_Universidad.sDepartamento_fl);
            ViewBag.sOrigen_fl = new SelectList(db.OrigenOtraUniversidades, "sOrigen_fl", "sDescripcion", gatbl_Universidad.sOrigen_fl);
            return View(gatbl_Universidad);
        }

        // GET: gatbl_Universidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Universidades gatbl_Universidades = db.gatbl_Universidades.Find(id);
            if (gatbl_Universidades == null)
            {
                return HttpNotFound();
            }
            ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion", gatbl_Universidades.sDepartamento_fl);
            ViewBag.sOrigen_fl = new SelectList(db.OrigenOtraUniversidades, "sOrigen_fl", "sDescripcion", gatbl_Universidades.sOrigen_fl);
            return View(gatbl_Universidades);
        }

        // POST: gatbl_Universidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_Universidades gatbl_Universidad)
        {
            if (ModelState.IsValid)
            {
                gatbl_Universidad.iEstado_fl = "1";
                gatbl_Universidad.iEliminado_fl = "1";
                gatbl_Universidad.sCreated_by = DateTime.Now.ToString();
                gatbl_Universidad.iConcurrencia_id = 1;

                db.Entry(gatbl_Universidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.sDepartamento_fl = new SelectList(db.Departamentos, "sDepartamento_fl", "sDescripcion", gatbl_Universidad.sDepartamento_fl);
            ViewBag.sOrigen_fl = new SelectList(db.OrigenOtraUniversidades, "sOrigen_fl", "sDescripcion", gatbl_Universidad.sOrigen_fl);
            return View(gatbl_Universidad);
        }

        // GET: gatbl_Universidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Universidades gatbl_Universidades = db.gatbl_Universidades.Find(id);
            if (gatbl_Universidades == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_Universidades);
        }

        // POST: gatbl_Universidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_Universidades gatbl_Universidades = db.gatbl_Universidades.Find(id);
            db.gatbl_Universidades.Remove(gatbl_Universidades);
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
