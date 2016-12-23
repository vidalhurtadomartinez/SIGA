using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using System.IO;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblDocumentoRelacionadosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblDocumentoRelacionados
        public ActionResult Index(int id)
        {
            ViewBag.DocumentID = id;
            //var docrelacionado = db.tblDocumentoRelacionado.Where(a => a.DocumentoID == id).OrderBy(a => a.NombreArchivo);

            var docrelacionado = ((ViewModels.Documento)Session["vDocumento"]).tblDocumentoRelacionados;

            return PartialView("_Index", docrelacionado.ToList());
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.DocumentID = id;
            var docrelacionado = db.tblDocumentoRelacionado.Where(a => a.DocumentoID == id);

            return PartialView("_List", docrelacionado.ToList());
        }

        public ActionResult Create(int DocumentID)
        {
            tblDocumentoRelacionado tblDocumentoRelacionado = new tblDocumentoRelacionado();
            tblDocumentoRelacionado.DocumentoID = DocumentID;

            return PartialView("_Create", tblDocumentoRelacionado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblDocumentoRelacionado tblDocumentoRelacionado)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = (HttpPostedFileBase)Session["UploadFile"];

                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/Adjunto"), fileName);

                    //file.SaveAs(physicalPath);

                    tblDocumentoRelacionado.NombreArchivo = fileName;
                    tblDocumentoRelacionado.Ubicacion = string.Format("Adjunto/{0}", fileName);
                }

                ViewModels.Documento documento = (ViewModels.Documento)Session["vDocumento"];

                int DetalleId = documento.tblDocumentoRelacionados.Count() + 1;
                tblDocumentoRelacionado.Id = -DetalleId;

                documento.tblDocumentoRelacionados.Add(tblDocumentoRelacionado);

                //db.tblDocumentoRelacionado.Add(tblDocumentoRelacionado);
                //db.SaveChanges();

                string url = Url.Action("Index", "tblDocumentoRelacionados", new { id = tblDocumentoRelacionado.DocumentoID });
                return Json(new { success = true, url = url });
            }

            return PartialView("_Create", tblDocumentoRelacionado);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblDocumentoRelacionado tblDocumentoRelacionado = db.tblDocumentoRelacionado.Find(id);

            tblDocumentoRelacionado tblDocumentoRelacionado = ((ViewModels.Documento)Session["vDocumento"]).tblDocumentoRelacionados.FirstOrDefault(t=>t.Id == id);
            if (tblDocumentoRelacionado == null)
            {
                return HttpNotFound();
            }            
            
            return PartialView("_Edit", tblDocumentoRelacionado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblDocumentoRelacionado tblDocumentoRelacionado)
        {
            if (ModelState.IsValid)
            {
                //var documentodetalle = db.tblDocumentoRelacionado.Find(tblDocumentoRelacionado.Id);
                tblDocumentoRelacionado documentoactual = ((ViewModels.Documento)Session["vDocumento"]).tblDocumentoRelacionados.FirstOrDefault(t => t.Id == tblDocumentoRelacionado.Id);
                
                if (documentoactual != null)
                {
                    documentoactual.Id = tblDocumentoRelacionado.Id;
                    documentoactual.NombreArchivo = tblDocumentoRelacionado.NombreArchivo;
                    documentoactual.Ubicacion = tblDocumentoRelacionado.Ubicacion;
                    documentoactual.Relacion = tblDocumentoRelacionado.Relacion;
                    documentoactual.DocumentoID = tblDocumentoRelacionado.DocumentoID;
                }

                HttpPostedFileBase file = (HttpPostedFileBase)Session["UploadFile"];

                if (file != null)
                {                    
                    var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/Adjunto"), fileName);

                    //file.SaveAs(physicalPath);

                    documentoactual.NombreArchivo = fileName;
                    documentoactual.Ubicacion = string.Format("Adjunto/{0}", fileName);
                }
                

                //db.Entry(documentodetalle).State = EntityState.Modified;
                //db.SaveChanges();

                string url = Url.Action("Index", "tblDocumentoRelacionados", new { id = tblDocumentoRelacionado.DocumentoID });
                return Json(new { success = true, url = url });
            }


            return PartialView("_Edit", new tblDocumentoRelacionado());
        }

        //public ActionResult Async()
        //{
        //    return View();
        //}

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Adjunto"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);

                    Session["UploadFile"] = file;                    
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    //// TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }

                    Session["UploadFile"] = null;
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblDocumentoRelacionado tblDocumentoRelacionado = db.tblDocumentoRelacionado.Find(id);
            tblDocumentoRelacionado tblDocumentoRelacionado = ((ViewModels.Documento)Session["vDocumento"]).tblDocumentoRelacionados.FirstOrDefault(t => t.Id == id);
            if (tblDocumentoRelacionado == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", tblDocumentoRelacionado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //tblDocumentoRelacionado tblDocumentoRelacionado = db.tblDocumentoRelacionado.Find(id);
            //db.tblDocumentoRelacionado.Remove(tblDocumentoRelacionado);
            //db.SaveChanges();

            var documento = (ViewModels.Documento)Session["vDocumento"];
            int DocumentID = 0;
            if(documento != null)
            {
                tblDocumentoRelacionado tblDocumentoRelacionado = documento.tblDocumentoRelacionados.FirstOrDefault(t => t.Id == id);

                if (tblDocumentoRelacionado != null)
                {
                    DocumentID = tblDocumentoRelacionado.DocumentoID;

                    if(id > 0)
                    {
                        documento.DocumentosEliminados.Add(tblDocumentoRelacionado);
                    }
                    
                    documento.tblDocumentoRelacionados.Remove(tblDocumentoRelacionado);
                }
            }
                       

            string url = Url.Action("Index", "tblDocumentoRelacionados", new { id = DocumentID });
            return Json(new { success = true, url = url });

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
