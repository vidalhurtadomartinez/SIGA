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
{[Autenticado]
    public class NivelProgramaAnaliticosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        [Permiso(Permiso = RolesPermisos.CONV_NivelProgramaAnalitico_VerListado)]
        public ActionResult Index()
        {
            return View(db.NivelProgramaAnaliticos.ToList());
        }

        [Permiso(Permiso = RolesPermisos.CONV_NivelProgramaAnalitico_VerDetalle)]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NivelProgramaAnalitico nivelProgramaAnalitico = db.NivelProgramaAnaliticos.Find(id);
            if (nivelProgramaAnalitico == null)
            {
                return HttpNotFound();
            }
            return View(nivelProgramaAnalitico);
        }

        [Permiso(Permiso = RolesPermisos.CONV_NivelProgramaAnalitico_CrearNuevo)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: NivelProgramaAnaliticos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sNivel_fl,sDescripcion,bActivo")] NivelProgramaAnalitico nivelProgramaAnalitico)
        {
            if (ModelState.IsValid)
            {
                db.NivelProgramaAnaliticos.Add(nivelProgramaAnalitico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nivelProgramaAnalitico);
        }

        [Permiso(Permiso = RolesPermisos.CONV_NivelProgramaAnalitico_Editar)]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NivelProgramaAnalitico nivelProgramaAnalitico = db.NivelProgramaAnaliticos.Find(id);
            if (nivelProgramaAnalitico == null)
            {
                return HttpNotFound();
            }
            return View(nivelProgramaAnalitico);
        }

        // POST: NivelProgramaAnaliticos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sNivel_fl,sDescripcion,bActivo")] NivelProgramaAnalitico nivelProgramaAnalitico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nivelProgramaAnalitico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nivelProgramaAnalitico);
        }

        [Permiso(Permiso = RolesPermisos.CONV_NivelProgramaAnalitico_Eliminar)]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NivelProgramaAnalitico nivelProgramaAnalitico = db.NivelProgramaAnaliticos.Find(id);
            if (nivelProgramaAnalitico == null)
            {
                return HttpNotFound();
            }
            return View(nivelProgramaAnalitico);
        }

        // POST: NivelProgramaAnaliticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NivelProgramaAnalitico nivelProgramaAnalitico = db.NivelProgramaAnaliticos.Find(id);
            db.NivelProgramaAnaliticos.Remove(nivelProgramaAnalitico);
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
