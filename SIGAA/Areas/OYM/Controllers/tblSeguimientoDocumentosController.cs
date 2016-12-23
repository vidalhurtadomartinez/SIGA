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
    public class tblSeguimientoDocumentosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblSeguimientoDocumentos
        public ActionResult Index()
        {
            //var tblDocumentoProceso = db.tblDocumentoProceso.Include(t => t.tblDocumento).Include(t => t.tblProceso).Include(t => t.tblCategoria);

            var tblDocumentoProceso = from dp in db.tblDocumentoProceso.Include(t => t.tblDocumento).Include(t => t.tblProceso).Include(t => t.tblCategoria)
                                      where (from d in db.tblDocumento
                                             where d.lVersion == dp.lDocumento_version
                                             //&& d.lEstado_id != 3
                                             select d.Id).Contains(dp.lDocumento_id)
                                      select dp;


            return View(tblDocumentoProceso.ToList());
        }

        private int NextCategoryProcess(int docproceso_id)
        {
            int nCategoria = -1;

            var documentoproceso = db.tblDocumentoProceso.Include(p => p.tblProceso).First(p => p.lDocumentoProceso_id == docproceso_id);

            if (documentoproceso.tblProceso.lTipoProceso_id == 1)
            {
                var detalleflujo = db.tblProcesoDetalleCategoria.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == documentoproceso.tblProceso.lProceso_id);

                var detalle = detalleflujo.Where(f => f.tblCategoria.lPrioridad > documentoproceso.tblCategoria.lPrioridad).OrderBy(f => f.tblCategoria.lPrioridad);

                if (detalle.Count() > 0)
                {
                    nCategoria = detalle.First().lCategoria_id;
                }
            }
            else
            {
                var detalleflujo = db.tblProcesoDetalle.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == documentoproceso.tblProceso.lProceso_id);

                var detalle = detalleflujo.Where(f => f.tblCategoria.lPrioridad > documentoproceso.tblCategoria.lPrioridad).OrderBy(f => f.tblCategoria.lPrioridad);

                if (detalle.Count() > 0)
                {
                    nCategoria = detalle.First().lCategoria_id;
                }
            }

            return nCategoria;
        }

        private int LevelLast(int docproceso_id)
        {
            int iLevel = -1;

            var documentoproceso = db.tblDocumentoProceso.Include(p => p.tblProceso).First(p => p.lDocumentoProceso_id == docproceso_id);

            if (documentoproceso.tblProceso.lTipoProceso_id == 1)
            {
                var detalleflujo = db.tblProcesoDetalleCategoria.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == documentoproceso.tblProceso.lProceso_id);

                iLevel = detalleflujo.Max(f => f.lCategoria_id);
            }
            else
            {
                var detalleflujo = db.tblProcesoDetalle.Include(p => p.tblProceso).Include(c => c.tblCategoria).Where(d => d.lProceso_id == documentoproceso.tblProceso.lProceso_id);

                iLevel = detalleflujo.Max(f => f.lCategoria_id);
            }

            return iLevel;
        }

        // GET: tblSeguimientoDocumentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSeguimientoDocumento tblSeguimientoDocumento = db.tblSeguimientoDocumento.Find(id);
            if (tblSeguimientoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblSeguimientoDocumento);
        }

        // GET: tblSeguimientoDocumentos/Create
        public ActionResult Create(int? id)
        {
            TempData["id"] = id;

            return View();
        }        

        // POST: tblSeguimientoDocumentos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSeguimientoDocumento tblSeguimientoDocumento, HttpPostedFileBase Documentoupload)
        {
            if (ModelState.IsValid)
            {
                if(TempData["id"] != null)
                {
                    var docpro = db.tblDocumentoProceso.Find(TempData["id"]);
                    int NextLevel = NextCategoryProcess(docpro.lDocumentoProceso_id);
                    int iLast = LevelLast(docpro.lDocumentoProceso_id);

                    tblSeguimientoDocumento.lCategoria_id = docpro.lCategoria_id;
                    tblSeguimientoDocumento.lDocumentoProceso_id = docpro.lDocumentoProceso_id;

                    if (Request.Form["bAprobar"] != null)
                    {
                        tblSeguimientoDocumento.OrigenRespuesta = "01";
                    }
                    else if (Request.Form["bObservacion"] != null)
                    {
                        tblSeguimientoDocumento.OrigenRespuesta = "02";
                    }
                    

                    tblSeguimientoDocumento.dtFechaRespuesta_dt = DateTime.Now;
                    tblSeguimientoDocumento.lUsuario_id = FrontUser.Get().iUsuario_id;

                    if (Documentoupload != null && Documentoupload.ContentLength > 0)
                    {
                        string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                        string fileName = string.Format("{0}_{1}", strDate, System.IO.Path.GetFileName(Documentoupload.FileName));
                        string pathFile = string.Format("{0}/Adjunto/DocumentoRespuesta/{1}", Server.MapPath("~"), fileName);

                        tblSeguimientoDocumento.sPathDocumento = string.Format("Adjunto/DocumentoRespuesta/{0}", fileName);
                        Documentoupload.SaveAs(pathFile);
                    }


                    db.tblSeguimientoDocumento.Add(tblSeguimientoDocumento);
                    //db.SaveChanges();

                    if (docpro.lCategoria_id == iLast && tblSeguimientoDocumento.OrigenRespuesta == "01")
                    {
                        var document = db.tblDocumento.Find(docpro.lDocumento_id);

                        document.lEstado_id = 3;
                        document.lVersion = docpro.lDocumento_version+1;

                        db.Entry(document).State = EntityState.Modified;
                    }

                    if (NextLevel != -1)
                    {
                        docpro.lCategoria_id = NextLevel;

                        db.Entry(docpro).State = EntityState.Modified;                        
                    }
                                        

                    db.SaveChanges();

                }
                
                return RedirectToAction("Index");
            }

            return View(tblSeguimientoDocumento);
        }

        // GET: tblSeguimientoDocumentos/Responder/5
        public ActionResult Responder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentoSeguimiento vDocumentoSeguimiento = new DocumentoSeguimiento();

            var docpro = db.tblDocumentoProceso.Find(id);

            //tblSeguimientoDocumento tblSeguimientoDocumento = db.tblSeguimientoDocumento.Find(id);

            tblDocumento documento = db.tblDocumento.Find(docpro.lDocumento_id);

            vDocumentoSeguimiento.tblDocumentoProceso = docpro;
            vDocumentoSeguimiento.tblDocumento = documento;

            if (documento == null)
            {
                return HttpNotFound();
            }
            return View(vDocumentoSeguimiento);
        }



        // GET: tblSeguimientoDocumentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSeguimientoDocumento tblSeguimientoDocumento = db.tblSeguimientoDocumento.Find(id);
            if (tblSeguimientoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblSeguimientoDocumento);
        }

        // POST: tblSeguimientoDocumentos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lSeguimientoDocumento_id,dtFechaRespuesta_dt,sComentario,lFormularioProceso_id,lEstadoRespuesta_id")] tblSeguimientoDocumento tblSeguimientoDocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSeguimientoDocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblSeguimientoDocumento);
        }

        // GET: tblSeguimientoDocumentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSeguimientoDocumento tblSeguimientoDocumento = db.tblSeguimientoDocumento.Find(id);
            if (tblSeguimientoDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tblSeguimientoDocumento);
        }

        // POST: tblSeguimientoDocumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSeguimientoDocumento tblSeguimientoDocumento = db.tblSeguimientoDocumento.Find(id);
            db.tblSeguimientoDocumento.Remove(tblSeguimientoDocumento);
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
