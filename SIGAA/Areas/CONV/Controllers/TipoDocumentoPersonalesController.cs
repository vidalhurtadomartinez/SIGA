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
    public class TipoDocumentoPersonalesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoPersonal_VerListado)]
        public ActionResult Index()
        {
            return View(db.TipoDocumentoPersonales.ToList());
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoPersonal_VerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            if (tipoDocumentoPersonal == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoPersonal);
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoPersonal_CrearNuevo)]
        public ActionResult Create()
        {
            return View(new TipoDocumentoPersonal() { bActivo = true });            
        }

        // POST: TipoDocumentoPersonales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoDocumentoPersonal_id,sDescripcion,bActivo")] TipoDocumentoPersonal tipoDocumentoPersonal)
        {
            if (ModelState.IsValid)
            {
                db.TipoDocumentoPersonales.Add(tipoDocumentoPersonal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDocumentoPersonal);
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoPersonal_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            if (tipoDocumentoPersonal == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoPersonal);
        }

        // POST: TipoDocumentoPersonales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoDocumentoPersonal_id,sDescripcion,bActivo")] TipoDocumentoPersonal tipoDocumentoPersonal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDocumentoPersonal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDocumentoPersonal);
        }

        [Permiso(Permiso = RolesPermisos.CONV_TipoDocumentoPersonal_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            if (tipoDocumentoPersonal == null)
            {
                return HttpNotFound();
            }
            return View(tipoDocumentoPersonal);
        }

        // POST: TipoDocumentoPersonales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDocumentoPersonal tipoDocumentoPersonal = db.TipoDocumentoPersonales.Find(id);
            db.TipoDocumentoPersonales.Remove(tipoDocumentoPersonal);
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
