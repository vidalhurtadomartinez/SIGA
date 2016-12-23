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
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblCategoriasController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_categoriaDeParcipante_puedeVerIndice)]
        public ActionResult Index()
        {
            return View(db.tblCategoria.ToList());
        }

        [Permiso(Permiso = RolesPermisos.OYM_categoriaDeParcipante_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategoria tblCategoria = db.tblCategoria.Find(id);
            if (tblCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tblCategoria);
        }

        [Permiso(Permiso = RolesPermisos.OYM_categoriaDeParcipante_puedeCrearNuevo)]
        public ActionResult Create()
        {
            return View(new tblCategoria { bActivo = true });
        }

        // POST: tblCategorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lCategoria_id,sDescripcion,lPrioridad,bActivo")] tblCategoria tblCategoria)
        {
            if (ModelState.IsValid)
            {
                db.tblCategoria.Add(tblCategoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCategoria);
        }

        [Permiso(Permiso = RolesPermisos.OYM_categoriaDeParcipante_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategoria tblCategoria = db.tblCategoria.Find(id);
            if (tblCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tblCategoria);
        }

        // POST: tblCategorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lCategoria_id,sDescripcion,lPrioridad,bActivo")] tblCategoria tblCategoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCategoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCategoria);
        }

        [Permiso(Permiso = RolesPermisos.OYM_categoriaDeParcipante_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategoria tblCategoria = db.tblCategoria.Find(id);
            if (tblCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tblCategoria);
        }

        // POST: tblCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCategoria tblCategoria = db.tblCategoria.Find(id);
            db.tblCategoria.Remove(tblCategoria);
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
