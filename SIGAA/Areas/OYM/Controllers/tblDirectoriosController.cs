using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SIGAA.Etiquetas;
using SIGAA.Commons;
using SIGAA.Models;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblDirectoriosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblDirectorios
        public ActionResult Index()
        {
            var tblDirectorio = db.tblDirectorio.Include(t => t.DirectorioPadre);
            return View(tblDirectorio.ToList());
        }

        //public JsonResult Index([DataSourceRequest] DataSourceRequest request, int? id)
        //{
        //    var result = GetDirectory().ToTreeDataSourceResult(request,
        //        e => e.EmployeeID,
        //        e => e.ReportsTo,
        //        e => id.HasValue ? e.ReportsTo == id : e.ReportsTo == null,
        //        e => e.ToEmployeeDirectoryModel()
        //    );

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [Permiso(Permiso = RolesPermisos.OYM_directorioDeArchivos_puedeVerIndice)]
        public JsonResult All([DataSourceRequest] DataSourceRequest request)
        {
            //var result = GetDirectory().ToTreeDataSourceResult(request,
            //    e => e.EmployeeID,
            //    e => e.ReportsTo,
            //    e => e.ToEmployeeDirectoryModel(request)
            //);

            //return Json(result, JsonRequestBehavior.AllowGet);


            var post = from p in db.tblDirectorio.Include(g => g.DirectorioPadre)
                       orderby p.sNombre ascending
                       where p.bActivo == true
                       select p;
            return Json(post.ToDataSourceResult(request, p => new
            {
                p.lDirectorio_id,
                p.sNombre,
                p.sDescripcion,
                p.lDirectorioPadre_id,
                p.bActivo
            }));
        }

        private string getPathRoot(int ParentId)
        {
            var strResultado = string.Empty;

            var dir = from d in db.tblDirectorio
                      where d.lDirectorio_id == ParentId
                      select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id};

            var item = dir.FirstOrDefault();

            if(item != null)
            {
                strResultado = item.Nombre;

                while (item.DirectorioPadre != null)
                {
                    dir = from d in db.tblDirectorio
                          where d.lDirectorio_id == item.DirectorioPadre
                          select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id };

                    item = dir.FirstOrDefault();

                    strResultado = item.Nombre + "/" + strResultado;
                }
            }                          


            return strResultado;
        }

        private string getCodeByLevel(int ParentId)
        {
            string strCode = string.Empty;

            var dir = from d in db.tblDirectorio
                      where d.lDirectorio_id == ParentId
                      select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id, Codigo = d.sCodigo };

            var item = dir.FirstOrDefault();

            var Cantidad = db.tblDirectorio.Where(c => c.lDirectorioPadre_id == ParentId).Count()+1;

            if (item != null)
            {
                strCode = item.Codigo;                
            }

            return strCode + Cantidad.ToString();
        }

        private int getLevel(int ParentId)
        {
            int levelresult = 0;

            var dir = from d in db.tblDirectorio
                      where d.lDirectorio_id == ParentId
                      select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id };

            var item = dir.FirstOrDefault();

            if(item != null)
            {
                levelresult++;

                while (item.DirectorioPadre != null)
                {
                    dir = from d in db.tblDirectorio
                          where d.lDirectorio_id == item.DirectorioPadre
                          select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id };

                    item = dir.FirstOrDefault();

                    if (item != null)
                        levelresult++;
                }
            }                

            return levelresult;
        }

        public JsonResult Destroy([DataSourceRequest] DataSourceRequest request, tblDirectorio directorio)
        {
            if (ModelState.IsValid)
            {
                //employeeDirectory.Delete(employee, ModelState);

                //tblDirectorio tblDirectorio = db.tblDirectorio.Find(directorio.lDirectorio_id);
                //db.tblDirectorio.Remove(tblDirectorio);
                //db.SaveChanges();

                directorio.bActivo = false;

                db.Entry(directorio).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { directorio }.ToTreeDataSourceResult(request, ModelState));
        }

        public JsonResult CreateNew([DataSourceRequest] DataSourceRequest request, tblDirectorio directorio)
        {
            if (ModelState.IsValid)
            {
                //directorio.Insert(directorio, ModelState);

                string directoryparent = getPathRoot(Convert.ToInt32(directorio.lDirectorioPadre_id));

                string pathroot = string.Format("{0}/Adjunto/{1}", Server.MapPath("~"), directoryparent);
                string pathString = string.Format("{0}/{1}", pathroot, directorio.sNombre);

                directorio.Nivel = getLevel(Convert.ToInt32(directorio.lDirectorioPadre_id));
                directorio.sCodigo = getCodeByLevel(Convert.ToInt32(directorio.lDirectorioPadre_id));
                directorio.lUsuario_id = FrontUser.Get().iUsuario_id;

                db.tblDirectorio.Add(directorio);
                db.SaveChanges();

                System.IO.Directory.CreateDirectory(pathString);
            }

            return Json(new[] { directorio }.ToTreeDataSourceResult(request, ModelState));
        }

        public JsonResult Update([DataSourceRequest] DataSourceRequest request, tblDirectorio directorio)
        {
            if (ModelState.IsValid)
            {
                //employeeDirectory.Update(employee, ModelState);

                var dir = db.tblDirectorio.Find(directorio.lDirectorio_id);
                dir.sNombre = directorio.sNombre;
                dir.sDescripcion = directorio.sDescripcion;
                dir.bActivo = directorio.bActivo;                

                db.Entry(dir).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { directorio }.ToTreeDataSourceResult(request, ModelState));
        }

        //private IEnumerable<tblDirectorio> GetDirectory()
        //{
        //    return employeeDirectory.GetAll();
        //}

        // GET: tblDirectorios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDirectorio tblDirectorio = db.tblDirectorio.Find(id);
            if (tblDirectorio == null)
            {
                return HttpNotFound();
            }
            return View(tblDirectorio);
        }

        // GET: tblDirectorios/Create
        public ActionResult Create()
        {
            ViewBag.lDirectorioPadre_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre");
            return View();
        }

        // POST: tblDirectorios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblDirectorio tblDirectorio)
        {
            if (ModelState.IsValid)
            {
                tblDirectorio.lUsuario_id = FrontUser.Get().iUsuario_id;
                db.tblDirectorio.Add(tblDirectorio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.lDirectorioPadre_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDirectorio.lDirectorioPadre_id);
            return View(tblDirectorio);
        }

        // GET: tblDirectorios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDirectorio tblDirectorio = db.tblDirectorio.Find(id);
            if (tblDirectorio == null)
            {
                return HttpNotFound();
            }
            ViewBag.lDirectorioPadre_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDirectorio.lDirectorioPadre_id);
            return View(tblDirectorio);
        }

        // POST: tblDirectorios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblDirectorio tblDirectorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblDirectorio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lDirectorioPadre_id = new SelectList(db.tblDirectorio, "lDirectorio_id", "sNombre", tblDirectorio.lDirectorioPadre_id);
            return View(tblDirectorio);
        }

        // GET: tblDirectorios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDirectorio tblDirectorio = db.tblDirectorio.Find(id);
            if (tblDirectorio == null)
            {
                return HttpNotFound();
            }
            return View(tblDirectorio);
        }

        // POST: tblDirectorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblDirectorio tblDirectorio = db.tblDirectorio.Find(id);
            db.tblDirectorio.Remove(tblDirectorio);
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
