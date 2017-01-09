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
    public class TipoDocumentoSolicitudesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoSolicitud_VerListado)]
        public ActionResult Index()
        {
            return View(db.TipoDocumentoSolicitudes.ToList());
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoSolicitud_VerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoSolicitud tipoDocumentoSolicitud = db.TipoDocumentoSolicitudes.Find(id);
            if (tipoDocumentoSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoSolicitud);
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoSolicitud_CrearNuevo)]
        public ActionResult Create()
        {
            return View(new TipoDocumentoSolicitud() { bActivo = true });            
        }

        // POST: TipoDocumentoSolicitudes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoDocumentoSolicitud_id,sDescripcion,bActivo")] TipoDocumentoSolicitud tipoDocumentoSolicitud)
        {
            if (ModelState.IsValid)
            {
                db.TipoDocumentoSolicitudes.Add(tipoDocumentoSolicitud);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDocumentoSolicitud);
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoSolicitud_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoSolicitud tipoDocumentoSolicitud = db.TipoDocumentoSolicitudes.Find(id);
            if (tipoDocumentoSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoSolicitud);
        }

        // POST: TipoDocumentoSolicitudes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoDocumentoSolicitud_id,sDescripcion,bActivo")] TipoDocumentoSolicitud tipoDocumentoSolicitud)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDocumentoSolicitud).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDocumentoSolicitud);
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoSolicitud_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoSolicitud tipoDocumentoSolicitud = db.TipoDocumentoSolicitudes.Find(id);
            if (tipoDocumentoSolicitud == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoSolicitud);
        }

        // POST: TipoDocumentoSolicitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDocumentoSolicitud tipoDocumentoSolicitud = db.TipoDocumentoSolicitudes.Find(id);
            db.TipoDocumentoSolicitudes.Remove(tipoDocumentoSolicitud);
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
