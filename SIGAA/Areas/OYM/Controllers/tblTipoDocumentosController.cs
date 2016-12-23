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
    public class tblTipoDocumentosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeDocumento_puedeVerIndice)]
        public ActionResult Index()
        {
            return View(db.tblTipoDocumentos.ToList());
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeDocumento_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumentos.Find(id);
            if (tblTipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoDocumento);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeDocumento_puedeCrearNuevo)]
        public ActionResult Create()
        {
            return View(new tblTipoDocumento() { bActivo = true });            
        }

        // POST: tblTipoDocumentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lTipoDocumento_id,sSigla,sTipoDocumento,sDescripcion,bActivo")] tblTipoDocumento tblTipoDocumento)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = db.tblTipoDocumentos.Where(t => t.sSigla == tblTipoDocumento.sSigla).Count() > 0;

                if (isDuplicate)
                {
                    ViewBag.Error = "La sigla no puede ser duplicada";

                    return View(tblTipoDocumento);
                }
                else
                {
                    db.tblTipoDocumentos.Add(tblTipoDocumento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                                
            }

            return View(tblTipoDocumento);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeDocumento_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumentos.Find(id);
            if (tblTipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoDocumento);
        }

        // POST: tblTipoDocumentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lTipoDocumento_id,sSigla,sTipoDocumento,sDescripcion,bActivo")] tblTipoDocumento tblTipoDocumento)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = db.tblTipoDocumentos.Where(t => t.sSigla == tblTipoDocumento.sSigla && t.lTipoDocumento_id != tblTipoDocumento.lTipoDocumento_id).Count() > 0;

                if (isDuplicate)
                {
                    ViewBag.Error = "La sigla no puede ser duplicada";

                    return View(tblTipoDocumento);
                }
                else
                {
                    db.Entry(tblTipoDocumento).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }
            return View(tblTipoDocumento);
        }

        [Permiso(Permiso = RolesPermisos.OYM_tipoDeDocumento_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumentos.Find(id);
            if (tblTipoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblTipoDocumento);
        }

        // POST: tblTipoDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblTipoDocumento tblTipoDocumento = db.tblTipoDocumentos.Find(id);
            db.tblTipoDocumentos.Remove(tblTipoDocumento);
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
