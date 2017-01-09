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
    public class NacionalidadesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        [Permiso(Permiso = RolesPermisos.CONV_Nacionalidades_VerListado)]
        public ActionResult Index()
        {
            return View(db.Nacionalidades.ToList());
        }

        [Permiso(Permiso = RolesPermisos.CONV_Nacionalidadess_VerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        [Permiso(Permiso = RolesPermisos.CONV_Nacionalidades_CrearNuevo)]
        public ActionResult Create()
        {
            return View(new Nacionalidad() { bActivo = true });
        }

        // POST: Nacionalidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lNacionalidad_id,sDescripcion,sGentilicio,bActivo")] Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.Nacionalidades.Add(nacionalidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nacionalidad);
        }

        [Permiso(Permiso = RolesPermisos.CONV_Nacionalidades_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        // POST: Nacionalidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lNacionalidad_id,sDescripcion,sGentilicio,bActivo")] Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nacionalidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nacionalidad);
        }

        [Permiso(Permiso = RolesPermisos.CONV_Nacionalidades_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        // POST: Nacionalidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nacionalidad nacionalidad = db.Nacionalidades.Find(id);
            db.Nacionalidades.Remove(nacionalidad);
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
