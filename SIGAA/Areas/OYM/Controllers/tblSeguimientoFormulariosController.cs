using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using SIGAA.Areas.OYM.ViewModels;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblSeguimientoFormulariosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblSeguimientoFormularios
        public ActionResult Index()
        {
            //var tblFormularioProceso = db.tblFormularioProceso.Include(t => t.tblFormulario).Include(t => t.tblProceso).Include(t => t.tblCategoria);

            var tblFormularioProceso = from dp in db.tblFormularioProceso.Include(t => t.tblFormulario).Include(t => t.tblProceso).Include(t => t.tblCategoria)
                                      where (from d in db.tblFormulario
                                             where d.lVersion == dp.lFormulario_version
                                             //&& d.lEstado_id != 3
                                             select d.Id).Contains(dp.lFormulario_id)
                                      select dp;

            return View(tblFormularioProceso.ToList());
        }

        private int NextCategoryProcess(int formproceso_id)
        {
            int nCategoria = -1;

            var formularioproceso = db.tblFormularioProceso.Include(p => p.tblProceso).First(p => p.lFormularioProceso_id == formproceso_id);

            if (formularioproceso.tblProceso.lTipoProceso_id == 1)
            {
                var detalleflujo = db.tblProcesoDetalleCategoria.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == formularioproceso.tblProceso.lProceso_id);

                var detalle = detalleflujo.Where(f => f.tblCategoria.lPrioridad > formularioproceso.tblCategoria.lPrioridad).OrderBy(f => f.tblCategoria.lPrioridad);

                if (detalle.Count() > 0)
                {
                    nCategoria = detalle.First().lCategoria_id;
                }
            }
            else
            {
                var detalleflujo = db.tblProcesoDetalle.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == formularioproceso.tblProceso.lProceso_id);

                var detalle = detalleflujo.Where(f => f.tblCategoria.lPrioridad > formularioproceso.tblCategoria.lPrioridad).OrderBy(f => f.tblCategoria.lPrioridad);

                if (detalle.Count() > 0)
                {
                    nCategoria = detalle.First().lCategoria_id;
                }
            }

            return nCategoria;
        }

        private int LevelLast(int formproceso_id)
        {
            int iLevel = -1;

            var formularioproceso = db.tblFormularioProceso.Include(p => p.tblProceso).First(p => p.lFormularioProceso_id == formproceso_id);

            if (formularioproceso.tblProceso.lTipoProceso_id == 1)
            {
                var detalleflujo = db.tblProcesoDetalleCategoria.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == formularioproceso.tblProceso.lProceso_id);

                iLevel = detalleflujo.Max(f => f.lCategoria_id);
            }
            else
            {
                var detalleflujo = db.tblProcesoDetalle.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == formularioproceso.tblProceso.lProceso_id);

                iLevel = detalleflujo.Max(f => f.lCategoria_id);
            }

            return iLevel;
        }

        // GET: tblSeguimientoFormularios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSeguimientoFormulario tblSeguimientoFormulario = db.tblSeguimientoFormularios.Find(id);
            if (tblSeguimientoFormulario == null)
            {
                return HttpNotFound();
            }
            return View(tblSeguimientoFormulario);
        }

        // GET: tblSeguimientoFormularios/Create
        public ActionResult Create(int? id)
        {
            TempData["id"] = id;

            return View();
        }

        // POST: tblSeguimientoFormularios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSeguimientoFormulario tblSeguimientoFormulario, HttpPostedFileBase Documentoupload)
        {
            if (ModelState.IsValid)
            {
                if (TempData["id"] != null)
                {
                    var formpro = db.tblFormularioProceso.Find(TempData["id"]);
                    int NextLevel = NextCategoryProcess(formpro.lFormularioProceso_id);
                    int iLast = LevelLast(formpro.lFormularioProceso_id);

                    tblSeguimientoFormulario.lCategoria_id = formpro.lCategoria_id;
                    tblSeguimientoFormulario.lFormularioProceso_id = formpro.lFormularioProceso_id;

                    tblSeguimientoFormulario.dtFechaRespuesta_dt = DateTime.Now;
                    tblSeguimientoFormulario.lUsuario_id = FrontUser.Get().iUsuario_id;

                    if (Documentoupload != null && Documentoupload.ContentLength > 0)
                    {
                        string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                        string fileName = string.Format("{0}_{1}", strDate, System.IO.Path.GetFileName(Documentoupload.FileName));
                        string pathFile = string.Format("{0}/Adjunto/FormularioRespuesta/{1}", Server.MapPath("~"), fileName);

                        tblSeguimientoFormulario.sPathDocumento = string.Format("Adjunto/FormularioRespuesta/{0}", fileName);
                        Documentoupload.SaveAs(pathFile);
                    }


                    db.tblSeguimientoFormularios.Add(tblSeguimientoFormulario);
                    db.SaveChanges();

                    if (formpro.lCategoria_id == iLast)
                    {
                        var document = db.tblFormulario.Find(formpro.lFormulario_id);

                        document.lEstado_id = 3;
                        document.lVersion = document.lVersion + 1;

                        db.Entry(document).State = EntityState.Modified;
                    }

                    if (NextLevel != -1)
                    {
                        formpro.lCategoria_id = NextLevel;

                        db.Entry(formpro).State = EntityState.Modified;
                    }


                    db.SaveChanges();

                }

                return RedirectToAction("Index");
            }

            return View(tblSeguimientoFormulario);
        }

        // GET: tblSeguimientoFormularios/Responder/5
        public ActionResult Responder(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FormularioSeguimiento vFormularioSeguimiento = new FormularioSeguimiento();

            var formpro = db.tblFormularioProceso.Find(id);

            //tblSeguimientoDocumento tblSeguimientoDocumento = db.tblSeguimientoDocumento.Find(id);

            tblFormulario formulario = db.tblFormulario.Find(formpro.lFormulario_id);

            vFormularioSeguimiento.tblFormularioProceso = formpro;
            vFormularioSeguimiento.tblFormulario = formulario;

            if (formulario == null)
            {
                return HttpNotFound();
            }
            return View(vFormularioSeguimiento);
        }

        // GET: tblSeguimientoFormularios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSeguimientoFormulario tblSeguimientoFormulario = db.tblSeguimientoFormularios.Find(id);
            if (tblSeguimientoFormulario == null)
            {
                return HttpNotFound();
            }
            return View(tblSeguimientoFormulario);
        }

        // POST: tblSeguimientoFormularios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lSeguimientoFormulario_id,dtFechaRespuesta_dt,sComentario,sPathDocumento,lFormularioProceso_id,lEstadoRespuesta_id")] tblSeguimientoFormulario tblSeguimientoFormulario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSeguimientoFormulario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblSeguimientoFormulario);
        }

        // GET: tblSeguimientoFormularios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSeguimientoFormulario tblSeguimientoFormulario = db.tblSeguimientoFormularios.Find(id);
            if (tblSeguimientoFormulario == null)
            {
                return HttpNotFound();
            }
            return View(tblSeguimientoFormulario);
        }

        // POST: tblSeguimientoFormularios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSeguimientoFormulario tblSeguimientoFormulario = db.tblSeguimientoFormularios.Find(id);
            db.tblSeguimientoFormularios.Remove(tblSeguimientoFormulario);
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
