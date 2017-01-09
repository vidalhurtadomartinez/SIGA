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
    [Autenticado]
    public class UnidadNegociosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        [Permiso(Permiso = RolesPermisos.CONV_UnidadDeNegocio_VerListado)]
        public ActionResult Index()
        {
            return View(db.UnidadNegocios.ToList());
        }

        [Permiso(Permiso = RolesPermisos.CONV_UnidadDeNegocio_VerDetalle)]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            if (unidadNegocio == null)
            {
                return HttpNotFound();
            }
            return View(unidadNegocio);
        }

        [Permiso(Permiso = RolesPermisos.CONV_UnidadDeNegocio_CrearNuevo)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadNegocios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lUNegocio_id,sDescripcion,bActivo")] UnidadNegocio unidadNegocio)
        {
            if (ModelState.IsValid)
            {
                db.UnidadNegocios.Add(unidadNegocio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unidadNegocio);
        }

        [Permiso(Permiso = RolesPermisos.CONV_UnidadDeNegocio_Editar)]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            if (unidadNegocio == null)
            {
                return HttpNotFound();
            }
            return View(unidadNegocio);
        }

        // POST: UnidadNegocios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lUNegocio_id,sDescripcion,bActivo")] UnidadNegocio unidadNegocio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidadNegocio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidadNegocio);
        }

        [Permiso(Permiso = RolesPermisos.CONV_UnidadDeNegocio_Eliminar)]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            if (unidadNegocio == null)
            {
                return HttpNotFound();
            }
            return View(unidadNegocio);
        }

        // POST: UnidadNegocios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UnidadNegocio unidadNegocio = db.UnidadNegocios.Find(id);
            db.UnidadNegocios.Remove(unidadNegocio);
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
