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
    public class tblFormularioRelacionadosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblFormularioRelacionados
        public ActionResult Index(int id)
        {
            ViewBag.FormID = id;
            //var docrelacionado = db.tblFormularioRelacionado.Where(a => a.FormularioID == id).OrderBy(a => a.NombreArchivo);

            var docrelacionado = ((ViewModels.Formulario)Session["vFormulario"]).tblFormularioRelacionados;

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
            ViewBag.FormID = id;
            var docrelacionado = db.tblFormularioRelacionado.Where(a => a.FormularioID == id);

            return PartialView("_List", docrelacionado.ToList());
        }

        public ActionResult Create(int DocumentID)
        {
            tblFormularioRelacionado tblFormularioRelacionado = new tblFormularioRelacionado();
            tblFormularioRelacionado.FormularioID = DocumentID;

            return PartialView("_Create", tblFormularioRelacionado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblFormularioRelacionado tblFormularioRelacionado)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = (HttpPostedFileBase)Session["UploadFormFile"];

                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/Adjunto"), fileName);

                    //file.SaveAs(physicalPath);

                    tblFormularioRelacionado.NombreArchivo = fileName;
                    tblFormularioRelacionado.Ubicacion = string.Format("Adjunto/{0}", fileName);
                }

                ViewModels.Formulario formulario = (ViewModels.Formulario)Session["vFormulario"];

                int DetalleId = formulario.tblFormularioRelacionados.Count() + 1;
                tblFormularioRelacionado.Id = -DetalleId;

                formulario.tblFormularioRelacionados.Add(tblFormularioRelacionado);

                //db.tblFormularioRelacionado.Add(tblFormularioRelacionado);
                //db.SaveChanges();

                string url = Url.Action("Index", "tblFormularioRelacionados", new { id = tblFormularioRelacionado.FormularioID });
                return Json(new { success = true, url = url });
            }

            return PartialView("_Create", tblFormularioRelacionado);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblFormularioRelacionado tblFormularioRelacionado = db.tblFormularioRelacionado.Find(id);

            tblFormularioRelacionado tblFormularioRelacionado = ((ViewModels.Formulario)Session["vFormulario"]).tblFormularioRelacionados.FirstOrDefault(t => t.Id == id);
            if (tblFormularioRelacionado == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Edit", tblFormularioRelacionado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblFormularioRelacionado tblFormularioRelacionado)
        {
            if (ModelState.IsValid)
            {                
                tblFormularioRelacionado documentoactual = ((ViewModels.Formulario)Session["vFormulario"]).tblFormularioRelacionados.FirstOrDefault(t => t.Id == tblFormularioRelacionado.Id);

                if (documentoactual != null)
                {
                    documentoactual.Id = tblFormularioRelacionado.Id;
                    documentoactual.NombreArchivo = tblFormularioRelacionado.NombreArchivo;
                    documentoactual.Ubicacion = tblFormularioRelacionado.Ubicacion;
                    documentoactual.Relacion = tblFormularioRelacionado.Relacion;
                    documentoactual.FormularioID = tblFormularioRelacionado.FormularioID;
                }

                HttpPostedFileBase file = (HttpPostedFileBase)Session["UploadFormFile"];

                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    //var physicalPath = Path.Combine(Server.MapPath("~/Adjunto"), fileName);

                    //file.SaveAs(physicalPath);

                    documentoactual.NombreArchivo = fileName;
                    documentoactual.Ubicacion = string.Format("Adjunto/{0}", fileName);
                }

                db.Entry(documentoactual).State = EntityState.Modified;
                db.SaveChanges();

                string url = Url.Action("Index", "tblFormularioRelacionados", new { id = tblFormularioRelacionado.FormularioID });
                return Json(new { success = true, url = url });
            }


            return PartialView("_Edit", new tblFormularioRelacionado());
        }

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

                    Session["UploadFormFile"] = file;
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

                    Session["UploadFormFile"] = null;
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
            //tblFormularioRelacionado tblFormularioRelacionado = db.tblFormularioRelacionado.Find(id);

            tblFormularioRelacionado tblFormularioRelacionado = ((ViewModels.Formulario)Session["vFormulario"]).tblFormularioRelacionados.FirstOrDefault(t => t.Id == id);
            if (tblFormularioRelacionado == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", tblFormularioRelacionado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //tblFormularioRelacionado tblFormularioRelacionado = db.tblFormularioRelacionado.Find(id);
            //db.tblFormularioRelacionado.Remove(tblFormularioRelacionado);
            //db.SaveChanges();

            var documento = (ViewModels.Formulario)Session["vFormulario"];
            int FormID = 0;
            if (documento != null)
            {
                tblFormularioRelacionado tblFormularioRelacionado = documento.tblFormularioRelacionados.FirstOrDefault(t => t.Id == id);

                if (tblFormularioRelacionado != null)
                {
                    FormID = tblFormularioRelacionado.FormularioID;

                    if (id > 0)
                    {
                        documento.FormulariosEliminados.Add(tblFormularioRelacionado);
                    }

                    documento.tblFormularioRelacionados.Remove(tblFormularioRelacionado);
                }
            }

            string url = Url.Action("Index", "tblFormularioRelacionados", new { id = FormID });
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
